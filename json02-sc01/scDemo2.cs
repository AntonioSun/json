////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

namespace Demo2
{
    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/10
        public static void Test1()
        {
            var template = Template.Parse(@"This is a \n{{ text; text + ""\n"" + text }} World {{""\n""}}from scriban!");
            var result = template.Render(new { text = "Hello" });
            Console.WriteLine("\n## Test2-1, Hello World with new line output");
            Console.WriteLine(result);
            ;
        }

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test2()
        {
            {
                var template = Template.Parse(@"

### member-accessor

x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
===
{{
x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
""\n""
x.t = ""2017-02-24T10:18:50.36""
x.t
""\n""
x
""\n""
date.now
}}
");
                var result = template.Render();
                Console.WriteLine(result);
            }
            {
                string json = @"[
          {
            'Brand': 'Nokia','Type' : 'Lumia 800',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '3.7'}
          },
          { 'Brand': 'Nokia','Type' : 'Lumia 900',
            'Specs':{'Storage' : '8GB', 'Memory': '512MB','Screensize' : '4.3' }
          },
          { 'Brand': 'HTC ','Type' : 'Titan II',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '4.7' }
          }
        ]";
                var template = Template.Parse(@"
{{
js = " + json + @"
js
""\n""
js[0].Brand
""\n""
js[1].Specs.Storage
}}
");
                var result = template.Render(new { text = "Hello" });
                Console.WriteLine("\n## Test2-2-1, json objects");
                Console.WriteLine(result);
            }
            {
                string json = @"[
  {
    ""Brand"": ""Nokia"",""Type"" : ""Lumia 800"",
    ""Specs"":{""Storage"" : ""16GB"", ""Memory"": ""512MB"",""Screensize"" : ""3.7""}
  },
  { ""Brand"": ""HTC "",""Type"" : ""Radar"",
    ""Specs"":{""Storage"" : ""8GB"", ""Memory"": ""512MB"",""Screensize"" : ""3.8"" }
  }
        ]";
                var template = Template.Parse(@"
{{
js = " + json + @"
js
""\n""
js[1].Brand
""\n""
js[0].Specs.Storage
}}
");
                var result = template.Render(new { text = "Hello" });
                Console.WriteLine("\n## Test2-2-2, json objects");
                Console.WriteLine(result);
            }
        }

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/9
        public static void Test3()
        {
            {
                var template = Template.Parse(@"This is {{ text }},{{""\n""}} and {{myfunction}} from scriban!");
                var model = new { text = "Hello Text" };
                var scriptObject = new ScriptObject();
                scriptObject.Import(model);
                // Import the following delegate to scriptObject.myfunction (would be accessible as a global function)
                scriptObject.Import("myfunction", new Func<string>(() => "Hello Func"));

                var context = new TemplateContext();
                context.PushGlobal(scriptObject);
                template.Render(context);
                context.PopGlobal();

                var result = context.Output.ToString();
                Console.WriteLine("\n## Test2-3, Customized functions");
                Console.WriteLine(result);
            }
        }

        /// ////////////////////////////////////////////////////////////////////////////
    }
}
