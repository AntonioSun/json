////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
//using Newtonsoft.Json.Linq; // for JObject

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

//using Util.Scriban;

namespace Demo2
{

    class Program
    {

        public static void Init()
        {
            Util.JsonFunctions.Register(Util.Scriban.globalFunctions);
        }
        public static void Test20()
        {
            Console.WriteLine("\n## Test2-0");
            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/400-builtins/400-builtins.txt
            var result = "";

            var template = Template.Parse(@"
### List of all the builtin functions:

{{
func dump_members
	for member in $0 | array.map 'key' | array.sort
		"" "" + member + ""\n""
	end
end~}}
object:
{{dump_members object}}
json:
{{dump_members json}}
");
            result = template.Render();
            Console.WriteLine(result);
        }

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/blob/master/doc/language.md
        public static void Test2A()
        {
            var repos = JsonConvert.DeserializeObject<Demo1.Repos>(Demo1.Program.json);

            var template = Template.Parse(@"{
                    ""total"": {{repos.total_count}},
                    ""items"": [
                    {
                {{ for item in repos.items }}
                        ""P"": {{item.full_name}},
                        ""O"": {{ item.owner.login | string.capitalize }}
                    },
                {{end}}
                    ]
                }"); //                         ""O"": {{ item | json.ownertojson }}
            var model = new { repos = repos };
            var context = new TemplateContext();
            context.PushGlobal(Util.Scriban.globalFunctions);

            var localFunction = new ScriptObject();
            localFunction.Import(model);
            context.PushGlobal(localFunction);

            template.Render(context);
            context.PopGlobal();

            var result = context.Output.ToString();

            Console.WriteLine("\n## Test2A, json var");
            Console.WriteLine(result);
            //Console.ReadKey();
        }
    }
}
