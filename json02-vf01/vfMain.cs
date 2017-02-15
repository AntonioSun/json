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
        Shapes.Program.Test();
        NewShapes.Program.Test();

        Polymorphism1.Test.Test0();
        PolymorphismOverriding.Test.Test0();
        PolymorphismHiding.Test.Test0();

        OverridingHiding.Program.Test();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();

    }
}