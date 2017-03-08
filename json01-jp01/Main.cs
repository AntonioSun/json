////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: JSONPath generic selection tool
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject

class ProgTest
{
    static void Main(string[] args)
    {
        // http://stackoverflow.com/a/11791989/2125837
        if (args.Length == 0)
        {
            Console.WriteLine("Usage:\n  jpsel JSONPath"); // Check for null array
            // http://stackoverflow.com/a/10286091/2125837
            Environment.Exit(0);
        }

        StringBuilder js = new StringBuilder();
        string s;
        while ((s = Console.ReadLine()) != null)
        {
            // https://www.dotnetperls.com/stringbuilder
            // https://www.dotnetperls.com/newline
            js.Append(s).Append(Environment.NewLine);
        }
        s = js.ToString();
        //Console.WriteLine(s);

        try
        {
            var o = JsonConvert.DeserializeObject<JObject>(s);
            Console.WriteLine(JsonConvert.SerializeObject(o.SelectTokens(args[0]), Formatting.Indented));
        }
        catch
        {
            var o = JsonConvert.DeserializeObject<JArray>(s);
            Console.WriteLine(JsonConvert.SerializeObject(o.SelectTokens(args[0]), Formatting.Indented));
        }

    }
}
