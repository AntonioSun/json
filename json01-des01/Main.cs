using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

class ProgTest
{
    static void Main(string[] args)
    {
        Account.DeserializeObject();

        MoreTests.DeserializeCollection();
        MoreTests.DeserializeDictionary();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}

// Deserialize an Object
// http://www.newtonsoft.com/json/help/html/DeserializeObject.htm

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public IList<string> Roles { get; set; }


    public static void DeserializeObject()
    {
        string json1 = @"{
          'Email': 'james@example.com',
          'Active': true,
          'CreatedDate': '2013-01-20T00:00:00Z',
          'Roles': [
            'User',
            'Admin'
          ]
        }";
        Account account = JsonConvert.DeserializeObject<Account>(json1);
        Console.WriteLine(account.Email);

        // Serialize an Object
        // http://www.newtonsoft.com/json/help/html/SerializeObject.htm
        string json = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json);
        // {
        //   "Email": "james@example.com",
        //   "Active": true,
        //   "CreatedDate": "2013-01-20T00:00:00Z",
        //   "Roles": [
        //     "User",
        //     "Admin"
        //   ]
        // }

        string json2 = @"{
          'Email': 'james@example.com',
          'Active': true,
          'CreatedDate': '2013-01-20T00:00:00Z',
          'Some': 1,
          'More': 2,
          'Fields': 'like this',
          'Roles': [
            'User',
            'Admin'
          ]
        }";
        Account account2 = JsonConvert.DeserializeObject<Account>(json2);
        if (account == account2) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
        if (account.Equals(account2)) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
        string json2r = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json2r);
        if (json == json2r) Console.WriteLine("Same"); 
        else Console.WriteLine("Not the same");
    }
}

public class MoreTests
{
    // Deserialize a Collection
    // http://www.newtonsoft.com/json/help/html/DeserializeCollection.htm
    public static void DeserializeCollection()
    {
        string json = @"['Starcraft','Halo','Legend of Zelda']";
        List<string> videogames = JsonConvert.DeserializeObject<List<string>>(json);
        Console.WriteLine("\n## Deserialize a Collection");
        Console.WriteLine(string.Join(", ", videogames.ToArray()));
        // Starcraft, Halo, Legend of Zelda
    }

    // Deserialize a Dictionary
    // http://www.newtonsoft.com/json/help/html/DeserializeDictionary.htm
    public static void DeserializeDictionary()
    {
        string json = @"{
          'href': '/account/login.aspx',
          'target': '_blank'
        }";

        Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        Console.WriteLine("\n## Deserialize a Dictionary");
        Console.WriteLine(htmlAttributes["href"]);
        // /account/login.aspx
        Console.WriteLine(htmlAttributes["target"]);
        // _blank
        Console.WriteLine(JsonConvert.SerializeObject(htmlAttributes, Formatting.Indented));
    }


}
