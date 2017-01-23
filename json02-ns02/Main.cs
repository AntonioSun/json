////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿// Introduction to C# Namespaces
// http://csharp-station.com/Tutorial/CSharp/Lesson06

// Namespace Declaration
using System;

// Listing 6-4. Calling Namespace Members: NamespaceCall.cs
namespace csharp_station
{
    using csharp_station.tutorial2;

    // nested namespace
    namespace tutorial
    {
        class myExample1
        {
            public static void myPrint1()
            {
                Console.WriteLine("First Example of calling another namespace member.");
            }
        }
    }


    // Program start class
    class NamespaceCalling
    {
        // Main begins program execution.
        public static void Main()
        {
            // Write to console
            tutorial.myExample1.myPrint1();
            tutorial.myExample2.myPrint2();

            // Directly Call the using namespace member
            myExample.myPrint(); 

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

// same namespace as nested namespace above
namespace csharp_station.tutorial
{
    class myExample2
    {
        public static void myPrint2()
        {
            Console.WriteLine("Second Example of calling another namespace member.");
        }
    }
}

// Listing 6-5. The using Directive: UsingDirective.cs
// C# Station Tutorial Namespace
namespace csharp_station.tutorial2
{
    class myExample
    {
        public static void myPrint()
        {
            Console.WriteLine("Example of using a using directive.");
        }
    }
}
