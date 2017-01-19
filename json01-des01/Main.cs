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
        string json = @"{
          'Email': 'james@example.com',
          'Active': true,
          'CreatedDate': '2013-01-20T00:00:00Z',
          'Roles': [
            'User',
            'Admin'
          ]
        }";
        Account account = JsonConvert.DeserializeObject<Account>(json);
        Console.WriteLine(account.Email);

        // Serialize an Object
        // http://www.newtonsoft.com/json/help/html/SerializeObject.htm
        string json2 = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json2);
        // {
        //   "Email": "james@example.com",
        //   "Active": true,
        //   "CreatedDate": "2013-01-20T00:00:00Z",
        //   "Roles": [
        //     "User",
        //     "Admin"
        //   ]
        // }
    }
}

