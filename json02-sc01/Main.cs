////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

using System;

class ProgTest
{
    static void Main(string[] args)
    {
        Demo1.Program.TestHelloWorld();
        Demo1.Program.Test100expressions();
        Demo1.Program.Test200statements();
        Demo1.Program.Test400builtins();

        Demo2.Program.Test1();
        Demo2.Program.Test2();
        Demo2.Program.Test3();

        Demo3.Program.Test0();
        Demo3.Program.Test0A();
        Demo3.Program.Test0B();
        Demo3.Program.Test1A();
        Demo3.Program.Test1B();
        //Demo3.Program.Test2();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
