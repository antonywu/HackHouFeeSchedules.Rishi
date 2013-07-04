using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace RESTfulAPI.Services
{
    public class JsonRepository
    {         
        public string GetAllJsons(string tableName, string department)
        {
            SqlDataReader rdr = null;
            SqlConnection conn = null;
            SqlCommand command = null;
            HttpContext context = null;
            String connectionString = string.Empty;
            String json = string.Empty;
            try
            {       
            connectionString = ConfigurationManager.ConnectionStrings["HackHou2008ConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);
            command = new SqlCommand("SELECT * FROM " + tableName,conn);
            context = HttpContext.Current;
            
            //command = new SqlCommand("GetJSON", conn);        
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.Add(new SqlParameter("@table_name", tableName));
            //if (department != null)
            //{
            //    command.Parameters.Add(new SqlParameter("@department", department));
            //}
            
            StringBuilder allJSONs = new StringBuilder();
            // ... SQL connection and command set up
            conn.Open();
            command.CommandTimeout = 3600;
            rdr = command.ExecuteReader();
           
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartArray();
            while (rdr.Read()) {
                int fieldcount = rdr.FieldCount; // count how many columns are in the row
                object[] values = new object[fieldcount]; // storage for column values
                rdr.GetValues(values); // extract the values in each column
                jsonWriter.WriteStartObject();
                for (int index = 0; index < fieldcount; index++)
                { // iterate through all columns
                    jsonWriter.WritePropertyName(rdr.GetName(index)); // column name
                    jsonWriter.WriteValue(values[index]); // value in column
                }
                jsonWriter.WriteEndObject();
             }
            jsonWriter.WriteEndArray();
            rdr.Close();
            json = sb.ToString();
            //context.Response.ContentType = "application/json";
            //context.Response.Write(sb);
            //End of method
        }
        catch (SqlException sqlException)
        { // exception
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Connection Exception: ");
            //context.Response.Write(sqlException.ToString() + "\n");          
            json = "Connection Exception: " + sqlException.ToString() + "\n";
        }
        finally
        {
            conn.Close(); // close the connection         
        }
        //return context;
                
        //while (rdr.Read())
        //{
        //    json += rdr["json"];
        //}
        return json;
        }
    }
}