////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

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

    public static string json = @"[
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

    public static void Test1()
	{

        var deserialized =
            JsonConvert.DeserializeObject<List<Phone>>(json);
         Console.WriteLine("\n## Deserialize Phone List");
         Console.WriteLine(JsonConvert.SerializeObject(deserialized));
         Console.WriteLine(deserialized[0].Brand);
         Console.WriteLine(deserialized[1].Specs.Storage);
         //Console.ReadKey();

         var template = Template.Parse(@"{{ phones[0].Brand }}"); // phones[1].Specs.Storage
         var model = new { phones = deserialized };
         var scriptObject = new ScriptObject();
         scriptObject.Import(model);
         // Import the following delegate to scriptObject.myfunction (would be accessible as a global function)
         //scriptObject.Import("serialize", new Func<string>(() => "Hello Func"));

         var context = new TemplateContext();
         context.PushGlobal(scriptObject);
         template.Render(context);
         context.PopGlobal();

         var result = context.Output.ToString();
         Console.WriteLine("\n## Test1, Customized function");
         Console.WriteLine(result);

    }
}
