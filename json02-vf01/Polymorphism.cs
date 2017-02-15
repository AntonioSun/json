////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# Polymorphism Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;

// http://www.akadia.com/services/dotnet_polymorphism.html

/// A method Foo() which is declared in the base class A and not redeclared in classes B or C is inherited in the two subclasses
namespace Polymorphism1
{
    class A
    {
        public void Foo() { Console.WriteLine("A::Foo()"); }
    }

    class B : A { }

    class Test
    {
        public static void Test0()
        {
            Console.WriteLine("= Polymorphism1");

            A a = new A();
            a.Foo();  // output --> "A::Foo()"

            B b = new B();
            b.Foo();  // output --> "A::Foo()"
        }
    }
}

/*

namespace Polymorphism2
{
    class A
    {
        public void Foo() { Console.WriteLine("A::Foo()"); }
    }

    class B : A
    {
        public void Foo() { Console.WriteLine("B::Foo()"); }
    }

    class Test
    {
        static void Test0(string[] args)
        {
            A a;
            B b;

            a = new A();
            b = new B();
            a.Foo();  // output --> "A::Foo()"
            b.Foo();  // output --> "B::Foo()"

            a = new B();
            a.Foo();  // output --> "A::Foo()"
        }
    }
}


There are two problems with this code.

- The output is not really what we, say from Java, expected. The method Foo() is a non-virtual method. C# requires the use of the keyword virtual in order for a method to actually be virtual. An example using virtual methods and polymorphism will be given in the next section.

- Although the code compiles and runs, the compiler produces a warning:
...\polymorphism.cs(11,15): warning CS0108: The keyword new is required on 'Polymorphism.B.Foo()' because it hides inherited member 'Polymorphism.A.Foo()'

*/

/// Only if a method is declared virtual, derived classes can override this method if they are explicitly declared 
/// to override the virtual base class method with the override keyword.
namespace PolymorphismOverriding 
{
    class A
    {
        public virtual void Foo() { Console.WriteLine("A::Foo()"); }
    }

    class B : A
    {
        public override void Foo() { base.Foo(); Console.WriteLine("B::Foo()"); }
    }

    class Test
    {
        public static void Test0()
        {
            Console.WriteLine("= PolymorphismOverriding");

            A a;
            B b;

            a = new A();
            b = new B();
            a.Foo();  // output --> "A::Foo()"
            b.Foo();  // output --> "A::Foo() & B::Foo()"

            a = new B();
            a.Foo();  // output --> "A::Foo() & B::Foo()"
        }
    }
}

namespace PolymorphismHiding
{
    class A
    {
        public void Foo() { Console.WriteLine("A::Foo()"); }
    }

    class B : A
    {
        public new void Foo() { base.Foo(); Console.WriteLine("B::Foo()"); }
    }

    class Test
    {
        public static void Test0()
        {
            Console.WriteLine("= PolymorphismHiding");

            A a;
            B b;

            a = new A();
            b = new B();
            a.Foo();  // output --> "A::Foo()"
            b.Foo();  // output --> "A::Foo() & B::Foo()"

            a = new B();
            a.Foo();  // output --> "A::Foo()"
        }
    }
}


/*

= Polymorphism1
A::Foo()
A::Foo()
= PolymorphismOverriding
A::Foo()
A::Foo()
B::Foo()
A::Foo()
B::Foo()
= PolymorphismHiding
A::Foo()
A::Foo()
B::Foo()
A::Foo()

*/

// http://stackoverflow.com/questions/3838553/overriding-vs-method-hiding

namespace OverridingHiding
{
    public class BaseClass
    {
        public void WriteNum()
        {
            Console.WriteLine(12);
        }
        public virtual void WriteStr()
        {
            Console.WriteLine("abc");
        }
    }

    public class DerivedClass : BaseClass
    {
        public new void WriteNum()
        {
            Console.WriteLine(42);
        }
        public override void WriteStr()
        {
            Console.WriteLine("xyz");
        }
    }
    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test()
        {
            Console.WriteLine();

            BaseClass isReallyBase = new BaseClass();
            BaseClass isReallyDerived = new DerivedClass();
            DerivedClass isClearlyDerived = new DerivedClass();

            isReallyBase.WriteNum(); // writes 12
            isReallyBase.WriteStr(); // writes abc
            isReallyDerived.WriteNum(); // writes 12
            isReallyDerived.WriteStr(); // writes xyz
            isClearlyDerived.WriteNum(); // writes 42
            isClearlyDerived.WriteStr(); // writes xyz

            Console.WriteLine();
        }
    }
}

/*

Hiding means that we have a completely different method. When we call WriteNum() on isReallyDerived then there's no way of knowing 
that there is a different WriteNum() on DerivedClass so it isn't called. It can only be called when we are dealing with the object 
as a DerivedClass.

Most of the time hiding is bad. Generally, either you should have a method as virtual if its likely to be changed in a derived class,
and override it in the derived class. There are however two things it is useful for:

1. Forward compatibility. If DerivedClass had a DoStuff() method, and then later on BaseClass was changed to add a DoStuff() method, 
(remember that they may be written by different people and exist in different assemblies) then a ban on member hiding would have 
suddenly made DerivedClass buggy without it changing. Also, if the new DoStuff() on BaseClass was virtual, then automatically making
that on DerivedClass an override of it could lead to the pre-existing method being called when it shouldn't. Hence it's good that 
hiding is the default (we use new to make it clear we definitely want to hide, but leaving it out hides and emits a warning on 
compilation).
2. Poor-man's covariance. Consider a Clone() method on BaseClass that returns a new BaseClass that's a copy of that created. In the
override on DerivedClass this will create a DerivedClass but return it as a BaseClass, which isn't as useful. What we could do is to
have a virtual protected CreateClone() that is overridden. In BaseClass we have a Clone() that returns the result of this - and all 
is well - in DerivedClass we hide this with a new Clone() that returns a DerivedClass. Calling Clone() on BaseClass will always return
a BaseClass reference, which will be a BaseClass value or a DerivedClass value as appropriate. Calling Clone() on DerivedClass will 
return a DerivedClass value, which is what we'd want in that context. There are other variants of this principle, however it should 
be noted that they are all pretty rare.

*/