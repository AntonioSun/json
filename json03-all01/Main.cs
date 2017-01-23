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
        DL_Demo2.ToTest.Test();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
