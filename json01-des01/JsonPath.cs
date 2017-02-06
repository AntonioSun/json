////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# SelectToken & JSONPath Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject

// SelectToken & JSONPath queries
// http://www.newtonsoft.com/json/help/html/SelectToken.htm
// http://goessner.net/articles/JsonPath/

public class JsonPath
{
    public static JObject o = JObject.Parse(@"{
  'Stores': [
    'Lambton Quay',
    'Willis Street'
  ],
  'Manufacturers': [
    {
      'Name': 'Acme Co',
      'Products': [
        {
          'Name': 'Anvil',
          'Price': 50
        }
      ]
    },
    {
      'Name': 'Contoso',
      'Products': [
        {
          'Name': 'Elbow Grease',
          'Price': 99.95
        },
        {
          'Name': 'Headlight Fluid',
          'Price': 4
        }
      ]
    }
  ]
}");

    public static void Demo1()
	{
        string name = (string)o.SelectToken("Manufacturers[0].Name");
        // Acme Co

        decimal productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");
        // 50

        string productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");
        // Elbow Grease

        Console.WriteLine("\n## Demo 1");
        Console.WriteLine(name);
        Console.WriteLine(productPrice);
        Console.WriteLine(productName);
    }

    public static void Demo2()
    {
        Console.WriteLine("\n## Demo 2");

        // manufacturer with the name 'Acme Co'
        JToken acme = o.SelectToken("$.Manufacturers[?(@.Name == 'Acme Co')]");

        Console.WriteLine(acme);
        // { "Name": "Acme Co", Products: [{ "Name": "Anvil", "Price": 50 }] }

        // name of all products priced 50 and above
        IEnumerable<JToken> pricyProducts = o.SelectTokens("$..Products[?(@.Price >= 50)].Name");

        foreach (JToken item in pricyProducts)
        {
            Console.WriteLine(item);
        }
        // Anvil
        // Elbow Grease

        //Console.ReadKey();
    }
}
