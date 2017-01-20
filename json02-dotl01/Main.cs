using System;
using System.Collections.Generic;
using System.Text;

using DotLiquid;

class ProgTest
{
    static void Main(string[] args)
    {
        Demo0();
        DemoFilter();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    static void Demo0()
    {
        var template = Template.Parse("Hello, {{ name }}!");
        var result = template.Render(Hash.FromAnonymousObject(new { name = "World" }));
        Console.WriteLine(result);
    }

    static void DemoFilter()
    {
        var template = Template.Parse("Hello, {{ name | upcase | escape }}!");
        var result = template.Render(Hash.FromAnonymousObject(new { name = "<World>" }));
        Console.WriteLine(result);
    }


}
