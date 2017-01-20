using System;

// demonstrates namespace use
// https://www.dotnetperls.com/namespace

/*
 * A namespace is an organization construct. It helps us find and understand how a code base is arranged. 
 * Namespaces are not essential for C# programs. They are usually used to make code clearer.
 * 
 * This example has namespaces with the identifiers A, B, C, D, E, and F. Namespaces B and C are nested inside namespace A. 
 * Namespaces D, E, and F are all at the top level of the compilation unit.
 * 
 * In the Program class, notice how the Main entry point uses the CClass, DClass, and FClass types. 
 * Because the using A.B.C and using D directives are present in namespace E, the Main method can directly use those types.
 * Also:
 * With FClass, the namespace must be specified explicitly because F is not included inside of E with a using directive.
 * 
 * Allowed namespace type names. In addition to using the normal alphanumeric names for namespaces, you can include 
 * the period separator in a namespace. This is a condensed version of having nested namespaces. E.g., F.F.
 * 
 */

using A.B.C;

namespace E
{
    using D;

    class Program
    {
        static void Main()
        {
            // Can access CClass type directly from A.B.C. because of "using A.B.C"
            CClass var1 = new CClass();

            // Can access DClass type from D.  because of "using D"
            DClass var2 = new DClass();

            // Must explicitely specify F namespace.
            F.FClass var3 = new F.FClass();
            F.F.FClass var4 = new F.F.FClass();

            // Display types.
            Console.WriteLine(var1);
            Console.WriteLine(var2);
            Console.WriteLine(var3);
            Console.WriteLine(var4);
            //A.B.C.CClass
            //D.DClass
            //F.FClass

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

namespace A
{
    namespace B
    {
        namespace C
        {
            public class CClass
            {
            }
        }
    }
}

namespace D
{
    public class DClass
    {
    }
}

namespace F
{
    public class FClass
    {
    }
}

namespace F.F
{
    public class FClass
    {
    }
}
