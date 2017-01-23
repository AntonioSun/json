////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿// Anonymous Method
// https://www.tutorialspoint.com/csharp/csharp_anonymous_methods.htm

using System;

namespace AnonymousMethod
{
    delegate void NumberChanger(int n);

    class TestDelegate
    {
        static int num = 10;
        public static void AddNum(int p)
        {
            num += p;
            Console.WriteLine("Named Method: {0}", num);
        }

        public static void MultNum(int q)
        {
            num *= q;
            Console.WriteLine("Named Method: {0}", num);
        }

        public static int getNum()
        {
            return num;
        }
        public static void Test()
        {
            Console.WriteLine("\n## Anonymous Method");
            //create delegate instances using anonymous method
            NumberChanger nc = delegate(int x)
            {
                Console.WriteLine("Anonymous Method: {0}", x);
            };

            //calling the delegate using the anonymous method 
            nc(10);

            //instantiating the delegate using the named methods 
            nc = new NumberChanger(AddNum);

            //calling the delegate using the named methods 
            nc(5);

            //instantiating the delegate using another named methods 
            nc = new NumberChanger(MultNum);

            //calling the delegate using the named methods 
            nc(2);
        }
    }
}
/*
Anonymous Method: 10
Named Method: 15
Named Method: 30
*/
