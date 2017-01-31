using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ProgTest
{
    static void Main(string[] args)
    {
        Demo1.Program.Test1A();
        Demo1.Program.Test1B();
        Demo1.Program.TestDataTable();
        Demo1.Program.TestDBTable();


        Util.Scriban.Test0();

        Util.Scriban.Init();
        // Import the following delegate to scriptObject.myfunction1 (would be accessible as a global function)
        Util.Scriban.Reg("myfunction1", new Func<string>(() => "Hello Func"));
        Util.Scriban.Test1();

        Demo1.Program.Test2A();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();

    }
}
