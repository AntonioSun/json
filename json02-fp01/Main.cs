////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ProgTest
{
    static void Main(string[] args)
    {
        DelegateAppl.TestDelegate.Test1();
        DelegateAppl.TestDelegate.Test2();
        DelegateAppl.PrintString.Test();

        GenericApplication.Tester.Test();
        GenericMethodAppl.Program.Test();
        GenericDelegateAppl.TestDelegate.Test();

        AnonymousMethod.TestDelegate.Test();

        ModernLanguageConstructs.Program1.Test();
        ModernLanguageConstructs.Program2.Test();
        ModernLanguageConstructs.Program3.Test();
        ModernLanguageConstructs.Program4.Test();
        ModernLanguageConstructs.Program5.Test();

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}
