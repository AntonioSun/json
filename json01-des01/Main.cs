////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;

class ProgTest
{
    static void Main(string[] args)
    {
        // In Main
        Account.DeserializeObject();

        MoreTests.DeserializeCollection();
        MoreTests.DeserializeDictionary();
        
        MoreTests.DeserializeDataSet();
        MoreTests.TestSqlGetDataTable();

        // From JsonDemo
        JsonDemo.JsonDemoTest();
        //JsonDemo.JsonDemo2();

        // From ExtraFields
        ExtraFieldsDemo.ExtraFieldsTest();

        // From Dynamic
        DynamicDemo.DynamicTest();

        // From DataTable
        DataTableCode.TestClass.Test();
        DataTableDemo.Test();

        // From JsonPath
        JsonPath.Demo1();
        JsonPath.Demo2();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}

// Deserialize an Object
// http://www.newtonsoft.com/json/help/html/DeserializeObject.htm

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public IList<string> Roles { get; set; }


    public static void DeserializeObject()
    {
        string json1 = @"{
          'Email': 'james@example.com',
          'Active': true,
          'CreatedDate': '2013-01-20T00:00:00Z',
          'Roles': [
            'User',
            'Admin'
          ]
        }";
        Account account = JsonConvert.DeserializeObject<Account>(json1);
        Console.WriteLine(account.Email);

        // Serialize an Object
        // http://www.newtonsoft.com/json/help/html/SerializeObject.htm
        string json = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json);
        // {
        //   "Email": "james@example.com",
        //   "Active": true,
        //   "CreatedDate": "2013-01-20T00:00:00Z",
        //   "Roles": [
        //     "User",
        //     "Admin"
        //   ]
        // }

        string json2 = @"{
          'Email': 'james@example.com',
          'Active': true,
          'CreatedDate': '2013-01-20T00:00:00Z',
          'Some': 1,
          'More': 2,
          'Fields': 'like this',
          'Roles': [
            'User',
            'Admin'
          ]
        }";
        Account account2 = JsonConvert.DeserializeObject<Account>(json2);
        if (account == account2) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
        if (account.Equals(account2)) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
        string json2r = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json2r);
        if (json == json2r) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
    }
}

public class MoreTests
{
    // Deserialize a Collection
    // http://www.newtonsoft.com/json/help/html/DeserializeCollection.htm
    public static void DeserializeCollection()
    {
        string json = @"['Starcraft','Halo','Legend of Zelda']";
        List<string> videogames = JsonConvert.DeserializeObject<List<string>>(json);
        Console.WriteLine("\n## Deserialize a Collection");
        Console.WriteLine(string.Join(", ", videogames.ToArray()));
        // Starcraft, Halo, Legend of Zelda
    }

    // Deserialize a Dictionary
    // http://www.newtonsoft.com/json/help/html/DeserializeDictionary.htm
    public static void DeserializeDictionary()
    {
        string json = @"{
          'href': '/account/login.aspx',
          'target': '_blank'
        }";

        Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        Console.WriteLine("\n## Deserialize a Dictionary");
        Console.WriteLine(htmlAttributes["href"]);
        // /account/login.aspx
        Console.WriteLine(htmlAttributes["target"]);
        // _blank
        Console.WriteLine(JsonConvert.SerializeObject(htmlAttributes, Formatting.Indented));
    }

    // Deserialize a DataSet
    // http://www.newtonsoft.com/json/help/html/DeserializeDataSet.htm
    public static void DeserializeDataSet()
    {
        string json = @"{
          'Table1': [
            {
              'id': 0,
              'item': 'item 0'
            },
            {
              'id': 1,
              'item': 'item 1'
            }
          ]
        }";

        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);

        DataTable dataTable = dataSet.Tables["Table1"];

        Console.WriteLine("\n## Deserialize a DataSet");
        Console.WriteLine(dataTable.Rows.Count);
        // 2

        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine(row["id"] + " - " + row["item"]);
        }
        // 0 - item 0
        // 1 - item 1
        Console.WriteLine(JsonConvert.SerializeObject(dataSet, Formatting.Indented));
    }

    public static void TestSqlGetDataTable()
    {
        DataTable dataTable = GetDataTable("SELECT top 5 name, recovery_model_desc FROM sys.databases order by name",
            "Data Source=(local);Initial Catalog=master;Integrated Security=True");
        Console.WriteLine(JsonConvert.SerializeObject(dataTable, Formatting.Indented));
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
}


