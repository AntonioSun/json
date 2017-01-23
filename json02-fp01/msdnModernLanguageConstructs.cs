// C# Delegates, Actions, Funcs, Lambdas
// https://blogs.msdn.microsoft.com/brunoterkaly/2012/03/02/c-delegates-actions-funcs-lambdaskeeping-it-super-simple/
// By Bruno Terkaly bterkaly@microsoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*

Simple delegate example

 * A delegate is a type that safely encapsulates a method, similar to a function pointer in C and C++. 
 * Unlike C function pointers, delegates are object-oriented, type safe, and secure. 
 * The type of a delegate is defined by the name of the delegate.

Part 1	Helps the compiler with type safety
Part 2	One of the methods associated with the delegate
Part 3	The other method associated with the delegate
Part 4	Our object used to demo delegates with
Part 5	Declare a delegate and attach a method from the demo object
Part 6	Declare a delegate and attach the other method from the demo object
Part 7	Exercise the first delegate. In other words, use it to do work.
Part 8	Exercise the second delegate. In other words, use it to do work.

 */

namespace ModernLanguageConstructs
{
    class Program1
    {
        // Part 1 - Explicit declaration of a delegate (helps a compiler ensure type safety)
        public delegate double delegateConvertTemperature(double sourceTemp);

        // A sample class to play with
        class TemperatureConverterImp
        {
            // Part 2 - Will be attached to a delegate later in the code
            public double ConvertToFahrenheit(double celsius)
            {
                return (celsius * 9.0 / 5.0) + 32.0;
            }

            //  Part 3 - Will be attached to a delegate later in the code
            public double ConvertToCelsius(double fahrenheit)
            {
                return (fahrenheit - 32.0) * 5.0 / 9.0;
            }
        }


        public static void Test()
        {
            Console.WriteLine("\n## Simple delegate example");

            //  Part 4 - Instantiate the main object
            TemperatureConverterImp obj = new TemperatureConverterImp();

            //  Part 5 - Intantiate delegate #1
            delegateConvertTemperature delConvertToFahrenheit =
                         new delegateConvertTemperature(obj.ConvertToFahrenheit);

            //  Part 6 - Intantiate delegate #2
            delegateConvertTemperature delConvertToCelsius =
                         new delegateConvertTemperature(obj.ConvertToCelsius);

            // Use delegates to accomplish work

            //  Part 7 - delegate #1
            double celsius = 0.0;
            double fahrenheit = delConvertToFahrenheit(celsius);
            string msg1 = string.Format("Celsius = {0}, Fahrenheit = {1}",
                                         celsius, fahrenheit);
            Console.WriteLine(msg1);
            // Celsius = 0, Fahrenheit = 32

            //  Part 8 - delegate #2
            fahrenheit = 212.0;
            celsius = delConvertToCelsius(fahrenheit);
            string msg2 = string.Format("Celsius = {0}, Fahrenheit = {1}",
                                         celsius, fahrenheit);
            Console.WriteLine(msg2);
            // Celsius = 100, Fahrenheit = 212
        }
    }
}

/*

C# Actions – More sugar, please
You can use the Action(Of T) delegate to pass a method as a parameter without explicitly declaring a custom delegate. The sugar here
is you don’t have to declare a delegate. The compiler is smart enough to figure out the proper types.

But you pay a price in terms of a limitation. The corresponding method action must not return a value. 
(In C#, the method must return void.)

Part 1	The Action syntax avoids the use of a declared delegate. Everything is inline.
Part 2	The Action syntax avoids the use of a declared delegate. Everything is inline.
Part 3	Execute the corresponding Action code

 */

namespace ModernLanguageConstructs
{
    class Program2
    {
        public static void Test()
        {
            Console.WriteLine("\n## C# Actions");

            // Part 1 - First action that takes an int and converts it to hex
            Action<int> displayHex = delegate(int intValue)
            {
                Console.WriteLine(intValue.ToString("X"));
            };

            // Part 2 - Second action that takes a hex string and 
            // converts it to an int
            Action<string> displayInteger = delegate(string hexValue)
            {
                Console.WriteLine(int.Parse(hexValue,
                    System.Globalization.NumberStyles.HexNumber));
            };

            // Part 3 - exercise Action methods
            displayHex(16);         // 10
            displayInteger("10");   // 16
        }
    }
}

/*

Func<> Delegates
This differs from Action<> in the sense that it supports parameters AND return values.

You can use this delegate to represent a method that can be passed as a parameter without explicitly declaring a custom delegate. 
The encapsulated method must correspond to the method signature that is defined by this delegate.

This means that the encapsulated method must have one parameter that is passed to it by value, and that it must return a value.

*/

namespace ModernLanguageConstructs
{
    class Program3
    {
        public static void Test()
        {
            Console.WriteLine("\n## Func<> Delegates");
            // Part 1 - First Func<> that takes an int and returns a string
            Func<int, string> displayHex = delegate(int intValue)
            {
                return (intValue.ToString("X"));
            };

            // Part 2 - Second Func<> that takes a hex string and 
            // returns an int
            Func<string, int> displayInteger = delegate(string hexValue)
            {
                return (int.Parse(hexValue,
                    System.Globalization.NumberStyles.HexNumber));
            };

            // Part 3 - exercise Func<> delegates
            Console.WriteLine(displayHex(16));         // 10
            Console.WriteLine(displayInteger("10"));   // 16
        }
    }
}

/*

Lambdas – Syntactic Sugar Squared

I’ve been staring at Lambdas for years and for whatever reason they don’t come natural to me. Maybe I need to spend more time in a 
functional language like F# to make them a natural construct.

A lambda expression is an anonymous function that can contain expressions and statements, and can be used to create delegates or 
expression tree types.

All lambda expressions use the lambda operator =>, which is read as “goes to”. The left side of the lambda operator specifies the 
input parameters (if any) and the right side holds the expression or statement block.

The lambda expression x => x * x is read “x goes to x times x.”

Part 1	Declare 2 lambda expressions
Part 2	Run them.

*/

namespace ModernLanguageConstructs
{
    class Program4
    {
        public static void Test()
        {
            Console.WriteLine("\n## Lambdas – Syntactic Sugar Squared");

            // Part 1 - An action and a lambda
            Action<int> displayHex = intValue =>
            {
                Console.WriteLine(intValue.ToString("X"));
            };

            Action<string> displayInteger = hexValue =>
            {
                Console.WriteLine(int.Parse(hexValue,
                    System.Globalization.NumberStyles.HexNumber));
            };

            // Part 2 - Use the lambda expressions
            displayHex(16);         // 10
            displayInteger("10");   // 16

        }
    }
}

/*

Lambdas and Queries

Lambda expressions can also be used to simplify queries.

*/

namespace ModernLanguageConstructs
{
    class Program5
    {
        public static void Test()
        {
            Console.WriteLine("\n## Lambdas and Queries");

            // Part 1 - ordinary list object
            List<string> listPets = new List<string>();

            // Part 2 - Queryable list object

            IQueryable<string> queryPets = listPets.AsQueryable();
            listPets.Add("dog");
            listPets.Add("cat");
            listPets.Add("iguana");

            // Part 3 - Lambda Expression (does not use curly braces)
            string result1 = listPets.First(x => x.StartsWith("d"));
            Console.WriteLine(result1);  // Prints "dog"

            // Part 4 - Lambda expressions using iQueryable interface
            string result2 = queryPets.First(x => x.StartsWith("ig"));
            Console.WriteLine(result2);  // Prints "iguana"

            // Part 5 - Lambda Statement (uses curly braces)
            //          Supports the return statement
            result1 = listPets.First(x => { return x.EndsWith("a"); });
            Console.WriteLine(result1);  // Prints "iguana"

            // Part 6 - Does not compile
            // A lambda expression with a statement body 
            // cannot be converted to an expression tree    
            // result2 = queryPets.First(x => { return x.EndsWith("e"); }); 

            // Part 7 - Does compile using the Func<T> syntax
            //          You can pass in a lambda expression and it 
            //          will be compiled to an Expression(Of TDelegate).
            string result3 = queryPets.First((Func<string, bool>)
                                         (x => { return x.EndsWith("g"); }));
            Console.WriteLine(result3);  // Prints "dog"


            // Part 8 - Convert to IQueryable
            IEnumerable<string> result4 =
                listPets.AsQueryable().Where(pet => pet.Length == 3);

            foreach (string pet in result4)
                Console.WriteLine(pet);  // Prints "dog" then "cat"
        }
    }
}
/*

dog
iguana
iguana
dog
dog
cat

*/
