////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

// JSON deserialization with JSON.net: basics
// http://dotnetbyexample.blogspot.ca/2012/01/json-deserialization-with-jsonnet.html

public class JsonDemo
{
    public class Specs
    {
        public string Storage { get; set; }
        public string Memory { get; set; }
        public string Screensize { get; set; }
    }

    public class Phone
    {
        public string Brand { get; set; }
        public string Type { get; set; }
        public Specs Specs { get; set; }
    }

    public static void JsonDemoTest()
	{
        string json = @"[
          {
            'Brand': 'Nokia','Type' : 'Lumia 800',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '3.7'}
          },
          {
            'Brand': 'Nokia', 'Type' : 'Lumia 710',
            'Specs':{'Storage' : '8GB','Memory': '512MB','Screensize' : '3.7'}
          },  
          { 'Brand': 'Nokia','Type' : 'Lumia 900',
            'Specs':{'Storage' : '8GB', 'Memory': '512MB','Screensize' : '4.3' }
          },
          { 'Brand': 'HTC ','Type' : 'Titan II',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '4.7' }
          },
          { 'Brand': 'HTC ','Type' : 'Radar',
            'Specs':{'Storage' : '8GB', 'Memory': '512MB','Screensize' : '3.8' }
          }
        ]";

        var deserialized =
            JsonConvert.DeserializeObject<List<Phone>>(json);
         Console.WriteLine("\n## Deserialize Phone List");
         Console.WriteLine(JsonConvert.SerializeObject(deserialized, Formatting.Indented));
	}
}
