using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ProgTest
{
    static void Main(string[] args)
    {
        Demo1.Program.TestHelloWorld();
        Demo1.Program.Test100expressions();
        Demo1.Program.Test200statements();
        Demo1.Program.Test400builtins();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
