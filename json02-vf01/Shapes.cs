////////////////////////////////////////////////////////////////////////////
// Porgram: 
// Purpose: C# virtual functions Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;

// http://stackoverflow.com/questions/1062102/practical-usage-of-virtual-functions-in-c-sharp

namespace Shapes
{
    public class Shape
    {
        // A few example members
        public int X { get; private set; }
        public int Y { get; private set; }

        // Virtual method
        public virtual void Draw()
        {
            Console.WriteLine(" Performing base class drawing tasks");
        }
    }

    class Circle : Shape
    {
        public override void Draw()
        {
            // Code to draw a circle...
            Console.WriteLine("Drawing a circle");
            base.Draw();
        }
    }

    class Rectangle : Shape
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public override void Draw()
        {
            // Code to draw a rectangle...
            Console.WriteLine("Drawing a rectangle");
            base.Draw();
        }
    }

    class Triangle : Shape
    {
        public override void Draw()
        {
            // Code to draw a triangle...
            Console.WriteLine("Drawing a triangle");
            base.Draw();
        }
    }

    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test()
        {
            Shape[] s = new Shape[3];

            s[0] = new Circle();
            s[1] = new Rectangle();
            s[2] = new Triangle();

            for (int i = 0; i < 3; i++)
            {
                s[i].Draw();
            }
            Console.WriteLine();
        }
    }
}

namespace NewShapes
{
    class NewCircle : Shapes.Circle
    {
        public override void Draw()
        {
            // Code to draw a Circle...
            Console.WriteLine("== Drawing a new Circle");
            base.Draw();
        }
    }

    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test()
        {
            Shapes.Shape[] s = new Shapes.Shape[2];

            s[0] = new Shapes.Circle();
            s[1] = new NewCircle();

            for (int i = 0; i < 2; i++)
            {
                s[i].Draw();
            }
            Console.WriteLine();
        }
    }
}

/*

Drawing a circle
 Performing base class drawing tasks
Drawing a rectangle
 Performing base class drawing tasks
Drawing a triangle
 Performing base class drawing tasks

Drawing a circle
 Performing base class drawing tasks
== Drawing a new Circle
Drawing a circle
 Performing base class drawing tasks

*/