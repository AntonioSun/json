////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

// https://api.github.com/search/repositories?q=json

namespace Demo2
{
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

    public class Demo
    {

        public static void Test()
        {
            var repos = JsonConvert.DeserializeObject<Repos>(Demo1.Demo.json);
            Console.WriteLine(JsonConvert.SerializeObject(repos, Formatting.Indented));
        }
    }
}

