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

namespace Util
{
    class Scriban
    {

        public static TemplateContext context;
        public static IScriptObject scriptObject;

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/9
        public static void Test0()
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
            Console.WriteLine("\n## Test0, Customized function 0");
            Console.WriteLine(result);
        }

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Init()
        {
            scriptObject = new ScriptObject();
            context = new TemplateContext();
        }


        /// ////////////////////////////////////////////////////////////////////////////
        public static void Reg(string funcName, Delegate funcDef)
        {
            // Import the following delegate to scriptObject (would be accessible as a global function)
            scriptObject.Import(funcName, funcDef);
        }

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test1()
        {

            var template = Template.Parse(@"This is {{ text }},{{""\n""}} and {{myfunction1}} from scriban!");
            var model = new { text = "Hello Text" };
            scriptObject.Import(model);

            context.PushGlobal(scriptObject);
            template.Render(context);
            context.PopGlobal();

            var result = context.Output.ToString();
            Console.WriteLine("\n## Test1, Customized function 1");
            Console.WriteLine(result);
        }

        public static string RunWith(Template template, object model, string funcName, Delegate funcDef)
        {
            var scriptObject = new ScriptObject();
            scriptObject.Import(model);
            // Import the following delegate to scriptObject (would be accessible as a global function)
            scriptObject.Import(funcName, funcDef);

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);
            template.Render(context);
            context.PopGlobal();

            return context.Output.ToString();
        }

        public static void Test2()
        {
            var result = RunWith(Template.Parse(@"This is {{ text }},{{""\n""}} and {{myfunction2}} from scriban!"),
                new { text = "Hello Text" }, "myfunction2", new Func<string>(() => "Hello Func"));
            Console.WriteLine("\n## Test2, Customized function 2");
            Console.WriteLine(result);
        }

    }
}
