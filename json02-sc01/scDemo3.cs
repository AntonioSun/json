////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using System.Text;

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

namespace Demo3
{

    // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#imports-a-net-object-instance
    public class MyObject
    {
        public MyObject()
        {
            Hello = "hello from property!";
        }

        // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#imports-functions-from-a-net-class
        public static string HelloFunc()
        {
            return "hello from method1 of plain object!";
        }

        public string Hello { get; set; }
    }

    // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#automatic-functions-import-from-scriptobject
    // We simply inherit from ScriptObject
    // All functions defined in the object will be imported
    public class MyCustomFunctions : ScriptObject
    {
        public static string Hello()
        {
            return "hello from method of ScriptObject!";
        }

        [ScriptMemberIgnore] // This method won't be imported
        public static void NotImported()
        {
            // ...
        }
    }

    /// <summary>
    /// String functions available through the object 'string' in scriban.
    /// </summary>
    public static class StringFunctions
    {

        public static string Upcase(string text)
        {
            return text.ToUpperInvariant();
        }

        public static string Downcase(string text)
        {
            return text.ToLowerInvariant();
        }

        public static string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text) || char.IsUpper(text[0]))
            {
                return text ?? string.Empty;
            }

            var builder = new StringBuilder(text);
            builder[0] = char.ToUpper(builder[0]);
            return builder.ToString();
        }

        public static void Register(ScriptObject builtins)
        {
            //if (builtins == null) throw new ArgumentNullException(nameof(builtins));

            var stringObject = ScriptObject.From(typeof(StringFunctions));

            builtins.SetValue("mystr", stringObject, true);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    // Class definition.
    public class Author
    {
        public string Name { get; set; }
    }

    public class Book 
    {
        public string Title { get; set; }
        public Author Author { get; set; }
    }


    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test0()
        {
            Console.WriteLine("\n## Test3-0");
            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/400-builtins/400-builtins.txt
            var result = "";

            var template = Template.Parse(@"
### List of all the functions:

{{
func dump_members
	for member in $0 | array.map 'key' | array.sort
		"" "" + member + ""\n""
	end
end~}}
object:
{{dump_members object}}
mystr:
{{dump_members mystr}}
");
            result = template.Render();
            Console.WriteLine(result);
        }
        public static void Test0A()
        {
            {
                // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#imports-functions-from-a-net-class
                var scriptObject1 = new ScriptObject();
                scriptObject1.Import(typeof(MyObject));

                var context = new TemplateContext();
                context.PushGlobal(scriptObject1);

                var template = Template.Parse("This is MyFunctions.Hello: `{{hello_func}}`");
                var result = template.Render(context);

                // Prints This is MyFunctions.Hello: `hello from method!`
                Console.WriteLine(result);
            }

            {
                // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#automatic-functions-import-from-scriptobject
                var scriptObject1 = new MyCustomFunctions();

                var context = new TemplateContext();
                context.PushGlobal(scriptObject1);

                var template = Template.Parse("This is MyFunctions.Hello: `{{hello}}`");
                var result = template.Render(context);

                // Prints This is MyFunctions.Hello: `hello from method!`
                Console.WriteLine(result);
            }

            {
                // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#imports-a-net-object-instance
                var scriptObject1 = new ScriptObject();
                // Here the renamer will just return a same member name as the original
                // hence importing .NET member name as-is
                scriptObject1.Import(new MyObject(), renamer: member => member.Name);

                var context = new TemplateContext();
                context.PushGlobal(scriptObject1);

                var template = Template.Parse("This is Hello: `{{Hello}}`");
                var result = template.Render(context);

                // Prints This is MyFunctions.Hello: `hello from method!`
                Console.WriteLine(result);
            }
        }

        public static void Test0B()
        {
            // https://github.com/lunet-io/scriban/blob/master/doc/runtime.md#imports-functions-from-a-net-class
            var scriptObject1 = new ScriptObject();
            scriptObject1.Import(typeof(StringFunctions));

            var context = new TemplateContext();
            context.PushGlobal(scriptObject1);

            var template = Template.Parse("This is StringFunctions.Upcase: `{{ \"is\" | upcase }}`");
            var result = template.Render(context);

            // Prints This is MyFunctions.Hello: `hello from method!`
            Console.WriteLine(result);
        }

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/12
        public static void Test1()
        {
            var globalFunction = new ScriptObject();
            // registerMyGlobalFunctions(globalFunction);
            StringFunctions.Register(globalFunction);

            // var template = Template.Parse(@"This {{ ""is"" | mystr.upcase }} {{ text | mystr.downcase }} from scriban!");
            var template = Template.Parse(@"This {{ ""is"" | string.capitalize }} {{ text | string.capitalize }} from scriban!");
            var model = new { text = "Bonjour le monde" };
            var context = new TemplateContext();
            context.PushGlobal(globalFunction);

            var localFunction = new ScriptObject();
            localFunction.Import(model);
            context.PushGlobal(localFunction);

            template.Render(context);
            context.PopGlobal();

            var result = context.Output.ToString();
            Console.WriteLine("\n## Test3-1, Customized functions");
            Console.WriteLine(result);
        }

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test2()
        {

            Author a1 = new Author { Name = "John" };
            // http://www.functionx.com/csharp/arrays/Lesson03.htm
            Book[] b = new Book[2];
            b[0] = new Book();
            b[0].Title="Book1";
            b[0].Author = a1;
            b[1] = new Book();
            b[1].Title = "Book2";
            b[1].Author = a1;

            var globalFunction = new ScriptObject();
            // registerMyGlobalFunctions(globalFunction);
            StringFunctions.Register(globalFunction);

            // var template = Template.Parse(@"This {{books[0].Title | mystr.downcase}} of {{ books[1].Author.Name | mystr.upcase }} is from scriban!");
            var template = Template.Parse(@"This {{books[0].Title | string.capitalize}} of {{ books[1].Author.Name | mystr.upcase }} is from scriban!");
            var model = new { books = b };
            var context = new TemplateContext();
            // https://github.com/lunet-io/scriban/issues/14#issuecomment-276928045
            //context.MemberRenamer = new DelegateMemberRenamer(name => name);
            context.PushGlobal(globalFunction);

            var localFunction = new ScriptObject();
            localFunction.Import(model);
            context.PushGlobal(localFunction);

            template.Render(context);
            context.PopGlobal();

            var result = context.Output.ToString();
            Console.WriteLine("\n## Test3-1, Customized functions");
            Console.WriteLine(result);
        }

        /// ////////////////////////////////////////////////////////////////////////////
    }
}
