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
    // The data structures are define here
    // Usually in a spearated file

    // Ref: 
    // https://api.github.com/search/repositories?q=DotLiquid&sort=stars&order=desc

    public class Owner
    {
        public string login { get; set; }
        public string url { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public Owner owner { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
        public bool fork { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string pushed_at { get; set; }
        public int size { get; set; }
        public int stargazers_count { get; set; }
        public int watchers_count { get; set; }
        public string language { get; set; }
        public int open_issues_count { get; set; }
        public double score { get; set; }
    }

    public class Repos
    {
        public int total_count { get; set; }
        public List<Item> items { get; set; }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    // More Class definitions
    public class Author
    {
        public string Name { get; set; }
    }

    public class Book
    {
        public string Title { get; set; }
        public Author Author { get; set; }
    }
}

namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    // The extension functions are define here
    // Usually in another spearated file

    /// <summary>
    /// Functions available through the object 'ghext' in scriban.
    /// </summary>
    public static class GhExt
    {
        public static string Upcase(string text)
        {
            return text.ToUpperInvariant();
        }

        public static string Downcase(string text)
        {
            return text.ToLowerInvariant();
        }


        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/7
        public static void Register(ScriptObject scriptObject)
        {
            //if (builtins == null) throw new ArgumentNullException(nameof(builtins));

            // Register functions available through the object 'ghext' in scriban
            var arrayObject = ScriptObject.From(typeof(GhExt));
            scriptObject.SetValue("ghext", arrayObject, true);

            // Import the following delegate (would be accessible as a global function)
            // myfunction
            scriptObject.Import("myfunction", new Func<string>(() => "Hello Func"));
            // serialize
            scriptObject.Import("serialize", new Func<Object, string>(x => JsonConvert.SerializeObject(x)));
        }
    }
}
