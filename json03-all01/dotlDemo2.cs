////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using DotLiquid;


namespace DL_Demo2
{

    public class Owner
    {
        public string login { get; set; }
        public string type { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public Owner owner { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
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


    public class ToTest
    {
        public static void Init()
        {
            Template.RegisterSafeType(typeof(Repos), Hash.FromAnonymousObject);
            // MUST Register the following as SafeType as well! 
            Template.RegisterSafeType(typeof(Item), Hash.FromAnonymousObject);
            Template.RegisterSafeType(typeof(Owner), Hash.FromAnonymousObject);
        }

        public static void Test1()
        {
            string myTemplate = @"
<p>{{ repos.total_count }} entries:</p>
  <ul>
{% for item in repos.items -%}
    <li>{{ item.owner.login | upcase }}, {{item.name}}, {{item.full_name}}</li>
{% endfor -%}
  </ul>";
            var repos = JsonConvert.DeserializeObject<Repos>(Demo1.Demo.json);
            var template = DotLiquid.Template.Parse(myTemplate);
            var MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(new { repos = repos }));
            Console.WriteLine("\n## DotLiquid Demo2.1");
            Console.WriteLine(MessageBody);
        }

        public static void Test2()
        {
            string myTemplate = @"
{
  ""total"": {{repos.total_count}},
  ""items"": [
    {
{% for item in repos.items -%}
      ""P"": {{item.full_name}},
      ""O"": {{ item.owner.login | upcase }}
    },
{% endfor -%}
  ]
}";
            var repos = JsonConvert.DeserializeObject<Repos>(Demo1.Demo.json);
            var template = DotLiquid.Template.Parse(myTemplate);
            var MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(new { repos = repos }));
            Console.WriteLine("\n## DotLiquid Demo2.2");
            Console.WriteLine(MessageBody);
        }

    }
}
