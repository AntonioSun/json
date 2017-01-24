////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;


class ProgTest
{
    static void Main(string[] args)
    {
        Console.WriteLine("## Demo1");
        Demo1.Demo.Test();
        Console.WriteLine("\n## Demo2");
        Demo2.Demo.Test();

        DL_Demo1.ToTest.Test();

        DL_Demo2.ToTest.Init();
        DL_Demo2.ToTest.Test1();
        DL_Demo2.ToTest.Test2();

        DL_Demo3.ToTest.Test1();
        DL_Demo3.ToTest.Test2();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
