////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

// The namespace should be Ext.<Module>.<Project>
namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    // For test in C# only

    /// ////////////////////////////////////////////////////////////////////////////
    // Class definitions
    public class Author
    {
        public string Name { get; set; }
    }

    public class Book
    {
        public string Title { get; set; }
        public Author Author { get; set; }
    }

    class Test
    {

        public static void Test1()
        {
            Author a1 = new Author { Name = "John" };
            Book[] b = new Book[2];
            b[0] = new Book();
            b[0].Title = "Book1";
            b[0].Author = a1;
            b[1] = new Book();
            b[1].Title = "Book2";
            b[1].Author = a1;

            var template = Template.Parse(@"This is {{ text }} => {{ text | ghext.upcase }},{{""\n""}} and {{myfunction}} from scriban!{{""\n""}}{{ books | serialize }}");
            var model = new { text = "Hello Text", books = b };

            var scriptObject = new ScriptObject();
            scriptObject.Import(model);
            GhExt.Register(scriptObject);

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);
            template.Render(context);
            context.PopGlobal();
            var result = context.Output.ToString();

            Console.WriteLine("\n## Test1, Customized functions");
            Console.WriteLine(result);
        }
    }
}
