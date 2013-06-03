﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
//using RESTfulAPI.Models;

namespace RESTfulAPI.Services
{
    public class JsonRepository
    {
        public string GetAllJsons(string tableName, string department)
        {
            SqlDataReader rdr = null;
            SqlConnection conn = null;
            SqlCommand command = null;
            var connectionString = string.Empty;
            var json = string.Empty;
            connectionString = "Server=.\\SQLEXPRESS2008;Database=HackHou2008;Integrated Security=SSPI";
           
            conn = new SqlConnection(connectionString);
            command = new SqlCommand("GetJSON", conn);
                
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@table_name", tableName));
            if (department != null)
            {
                command.Parameters.Add(new SqlParameter("@department", department));
            }
                

            conn.Open();
            rdr = command.ExecuteReader();
                
            while (rdr.Read())
            {
                json += rdr["json"];
            }
            return json;
        }
    }
}