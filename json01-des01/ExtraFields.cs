////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

// Handling extra members when deserializing with Json.net
// http://stackoverflow.com/questions/25395765/
// To retain those extra fields that are not part of the class definition.

public class ExtraFieldsDemo
{
    public class Person
    {
        //[DataMember]
        string name;
        //[DataMember]
        int age;
        //[DataMember]
        int height;

        [JsonExtensionData]
        public IDictionary<string, object> other;
    }

    public static void ExtraFieldsTest()
	{
        string json = @"{
            'name':'Chris',
            'age':100,
            'birthplace':'UK',
            'height':170,
            'birthdate':'08/08/1913',
            'more_fields': {
                '_unknown_field_name_1': 'some value',
                '_unknown_field_name_2': 'some value'
            }
        }";
        // The fields "birthdate", "birthplace" and "more_fields" are not part of the Person class. 

        var person = JsonConvert.DeserializeObject<Person>(json);
        Console.WriteLine("\n## Deserialize with ExtraFields");
        Console.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));
        //{
        //  "name": "Chris",
        //  "age": 100,
        //  "birthplace": "UK",
        //  "height": 170,
        //  "birthdate": "08/08/1913",
        //  "more_fields": {
        //    "_unknown_field_name_1": "some value",
        //    "_unknown_field_name_2": "some value"
        //  }
        //}
        Console.WriteLine(person.other);
        Console.WriteLine(person.other["birthplace"]);
        Console.WriteLine(person.other["birthdate"]);
        //System.Collections.Generic.Dictionary`2[System.String,System.Object]
        //UK
        //08/08/1913
    }
}
