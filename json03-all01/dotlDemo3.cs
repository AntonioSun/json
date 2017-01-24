////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject

using DotLiquid;


namespace DL_Demo3
{
    public class ToTest
    {
        public static void Test1()
        {
            Console.WriteLine("\n## DotLiquid Demo3.1");

            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Column1");
            dataTable.Columns.Add("Column2");

            System.Data.DataRow dataRow = dataTable.NewRow();
            dataRow["Column1"] = "Hello";
            dataRow["Column2"] = "World";

            dataTable.Rows.Add(dataRow);

            string json = JsonConvert.SerializeObject(dataTable);
            Console.WriteLine(json);

            var deserialized = JsonConvert.DeserializeObject(json);
            Console.WriteLine(deserialized);

            string myTemplate = @"
[
    {
{% for tbr in tb -%}
      ""N"": {{tbr.Column1}},
      ""M"": {{tbr.Column2}}
    },
{% endfor -%}
]";

            var template = DotLiquid.Template.Parse(myTemplate);
            var MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(new { tb = deserialized }));
            Console.WriteLine(MessageBody);

        }

        public static string GetJsonTable()
        {
            DataTable dataTable = GetDataTable("SELECT top 5 name, recovery_model_desc FROM sys.databases order by name",
                "Data Source=(local);Initial Catalog=master;Integrated Security=True");
            return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
        }

        public static DataTable GetDataTable(string _SqlCommand, string _ConnectionStr)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(_ConnectionStr))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                connection.Open();
                cmd.CommandText = _SqlCommand;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@City", txtCity.Text);

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dataTable.Load(dr);
                }
            }
            return dataTable;
        }

        public static void Test2()
        {
            Console.WriteLine("\n## DotLiquid Demo3.2");

            string myTemplate = @"
[
    {
{% for tbr in tb -%}
      ""N"": {{tbr.name}},
      ""M"": {{tbr.recovery_model_desc}}
    },
{% endfor -%}
]";
            string myJson = GetJsonTable();
            var parsed = JsonConvert.DeserializeObject(myJson);
            Console.WriteLine(parsed);

            //Template.RegisterSafeType(typeof(JObject), Hash.FromAnonymousObject);

            var template = DotLiquid.Template.Parse(myTemplate);
            var MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(new { tb = parsed }));
            Console.WriteLine(MessageBody);
        }
    }
}
