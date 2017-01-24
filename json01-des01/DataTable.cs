////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject


// Convert a data row to a JSON object
// By dbc, http://stackoverflow.com/a/33400729/2125837

namespace DataTableCode
{
    public class Values
    {
        public int ValID { get; set; }
        public string Type { get; set; }
    }

    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Values Values { get; set; }
    }

    public class TestClass
    {
        static DataTable MakeTable()
        {
            var table = new DataTable();

            table.Columns.Add("America", typeof(Place));
            table.Columns.Add("Africa", typeof(Place));
            table.Columns.Add("Japan", typeof(Place));

            DataRow row = table.NewRow();

            row["America"] = JsonConvert.DeserializeObject<Place>(@"{""Id"":1,""Title"":""Ka""}");
            row["Africa"] = JsonConvert.DeserializeObject<Place>(@"{""Id"":2,""Title"":""Sf""}");
            row["Japan"] = JsonConvert.DeserializeObject<Place>(@"{""Id"":3,""Title"":""Ja"",""Values"":{""ValID"":4,""Type"":""Okinawa""}}");
            table.Rows.Add(row);
            return table;

        }

        public static void Test()
        {
            var datatable = MakeTable();

            var objType = JArray.FromObject(datatable);
            var js = objType.ToString();

            Console.WriteLine(js); // Outputs abbreviated JSON as desired.
        }
    }
}

public class DataTableDemo
{

    public static void Test()
    {
        Console.WriteLine("\n## Deserialize DataTable");

        System.Data.DataTable dataTable = new System.Data.DataTable();
        dataTable.Columns.Add("Column1");
        dataTable.Columns.Add("Column2");

        System.Data.DataRow dataRow = dataTable.NewRow();
        dataRow["Column1"] = "Hello";
        dataRow["Column2"] = "World";

        dataTable.Rows.Add(dataRow);

        // X string json = JsonConvert.SerializeObject(dataTable);
        // That only gives emtpy string

        var objType = JArray.FromObject(dataTable);
        string json = objType.ToString();
        Console.WriteLine(json);
        // http://stackoverflow.com/a/2979938/2125837
        Console.WriteLine(JsonConvert.SerializeObject(dataTable, Formatting.Indented));

        // X var deserialized = JsonConvert.DeserializeObject<JObject>(json);
        // will give Unable to cast object of type 'Newtonsoft.Json.Linq.JObject' to type 'Newtonsoft.Json.Linq.JArray'
        // Ref: http://stackoverflow.com/a/33495714/2125837 by Racil Hilan
        var deserialized = JsonConvert.DeserializeObject(json);
        Console.WriteLine(deserialized);
    }
}
