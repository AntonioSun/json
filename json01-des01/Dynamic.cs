using System;
using System.Collections.Generic;
using System.Linq; // for .SelectMany, etc

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject

// C# JSON.net, DeSerializate Dynamically
// http://stackoverflow.com/questions/17721755/parsing-dynamic-json-string-into-string-in-c-sharp-using-json-net
// Dealing with dynamic JSON string

public class DynamicDemo
{

    public static void DynamicTest()
	{
        Console.WriteLine("\n## Deserialize from dynamic JSON string");

        // I'd recommend parsing the dynamic json string as a JObject
        var parseTree = JsonConvert.DeserializeObject<JObject>(
            "{ a: 2, b: \"a string\", c: 1.75, d: {\"a\":2, \"b\":\"a string\", \"c\":3 }, e: [{a:1}, {b:2, c:3 }] }");
        foreach (var prop in parseTree.Properties())
        {
            Console.WriteLine(prop.Name + ": " + prop.Value.ToObject<object>());
        }
        //a: 2
        //b: a string
        //c: 1.75
        //d: {
        //  "a": 2,
        //  "b": "a string",
        //  "c": 3
        //}
        //e: [
        //  {
        //    "a": 1
        //  },
        //  {
        //    "b": 2,
        //    "c": 3
        //  }
        //]
        Console.WriteLine(parseTree["b"]);  // a string
        Console.WriteLine(parseTree["d"]);
        //{
        //  "a": 2,
        //  "b": "a string",
        //  "c": 3
        //}
        Console.WriteLine(parseTree);
        //{
        //  "a": 2,
        //  "b": "a string",
        //  "c": 1.75,
        //  "d": {
        //    "a": 2,
        //    "b": "a string",
        //    "c": 3
        //  }
        //}
        Console.WriteLine(parseTree["d"]["a"]); // 2
        Console.WriteLine(parseTree["e"][1]["c"]); // 3
        Console.WriteLine();

        // Another JObject example
        var parsed = JsonConvert.DeserializeObject<JObject>("{\"name\":{\"a\":2, \"b\":\"a string\", \"c\":3 } }");
        foreach (var property in parsed.Properties())
        {
            Console.WriteLine(property.Name);
            foreach (var innerProperty in ((JObject)property.Value).Properties())
            {
                Console.WriteLine("\t{0}: {1}", innerProperty.Name, innerProperty.Value.ToObject<object>());
            }
        }
        //name
        //    a: 2
        //    b: a string
        //    c: 3
        Console.WriteLine();

        // When dealing with a JSON array, parse instead as JArray and then look at each JObject in the array
        var properties = JsonConvert.DeserializeObject<JArray>("[{a:1}, {b:2, c:3 }]")
            .SelectMany(o => ((JObject)o).Properties())
            .ToArray();
        foreach (var prop in properties)
        {
            Console.WriteLine(prop.Name + ": " + prop.Value.ToObject<object>());
        }
        //a: 1
        //b: 2
        //c: 3
    }
}
