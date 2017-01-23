////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿// C# - Delegates
// https://www.tutorialspoint.com/csharp/csharp_delegates.htm

using System;
using System.IO;

// Instantiating Delegates
/*
 * Once a delegate type is declared, a delegate object must be created with the new keyword and be associated 
 * with a particular method. When creating a delegate, the argument passed to the new expression is written 
 * similar to a method call, but without the arguments to the method. 
 */
delegate int NumberChanger(int n);
namespace DelegateAppl
{
    class TestDelegate
    {
        static int num = 10;
        public static void Init()
        {
            num = 10;
        }
        
        public static int AddNum(int p)
        {
            num += p;
            return num;
        }

        public static int MultNum(int q)
        {
            num *= q;
            return num;
        }
        public static int getNum()
        {
            return num;
        }
        public static void Test1()
        {
            Console.WriteLine("## Instantiating Delegates");
            //create delegate instances
            NumberChanger nc1 = new NumberChanger(DelegateAppl.TestDelegate.AddNum);
            NumberChanger nc2 = new NumberChanger(DelegateAppl.TestDelegate.MultNum);

            //calling the methods using the delegate objects
            nc1(25);
            Console.WriteLine("Value of Num: {0}", DelegateAppl.TestDelegate.getNum());
            // Value of Num: 35
            nc2(5);
            Console.WriteLine("Value of Num: {0}", DelegateAppl.TestDelegate.getNum());
            // Value of Num: 175
        }

        // Multicasting of a Delegate
        /*
         * Delegate objects can be composed using the "+" operator. A composed delegate calls the two delegates it was composed from. 
         * Only delegates of the same type can be composed. The "-" operator can be used to remove a component delegate from a composed delegate.
         * 
         * Using this property of delegates you can create an invocation list of methods that will be called when a delegate is invoked. 
         * This is called multicasting of a delegate. 
        */

        public static void Test2()
        {
            Console.WriteLine("\n## Multicasting of a Delegate");
            Init();
            //create delegate instances
            NumberChanger nc;
            NumberChanger nc1 = new NumberChanger(AddNum);
            NumberChanger nc2 = new NumberChanger(MultNum);
            nc = nc1;
            nc += nc2;

            //calling multicast
            nc(5);
            Console.WriteLine("Value of Num: {0}", getNum());
            // Value of Num: 75
        }
    }
}

// Using Delegates
/*
 * The following example demonstrates the use of delegate. The delegate printString can be used to reference method 
 * that takes a string as input and returns nothing.
 * 
 * We use this delegate to call two methods, the first prints the string to the console, and the second one prints it to a file
*/
namespace DelegateAppl
{
    class PrintString
    {
        static FileStream fs;
        static StreamWriter sw;

        // delegate declaration
        public delegate void printString(string s);

        // this method prints to the console
        public static void WriteToScreen(string str)
        {
            Console.WriteLine("The String is: {0}", str);
        }

        //this method prints to a file
        public static void WriteToFile(string s)
        {
            fs = new FileStream("d:\\message.txt",
            FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        // this method takes the delegate as parameter and uses it to
        // call the methods as required
        public static void sendString(printString ps)
        {
            ps("Hello World");
        }
        public static void Test()
        {
            printString ps1 = new printString(WriteToScreen);
            printString ps2 = new printString(WriteToFile);
            sendString(ps1);
            // The String is: Hello World
            sendString(ps2);
            // > type d:\message.txt && del d:\message.txt
            // Hello World
        }
    }
}
