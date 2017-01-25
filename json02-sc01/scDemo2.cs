////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Scriban;

namespace Demo2
{
    class Program
    {

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test1()
        {
            var template = Template.Parse(@"This is a \n{{ text }} World \nfrom scriban!");
            var result = template.Render(new { text = "Hello" });
            Console.WriteLine("\n## Test1");
            Console.WriteLine(result);
            ;
        }

        /// ////////////////////////////////////////////////////////////////////////////
        public static void Test2()
        {
            {
                var template = Template.Parse(@"

### member-accessor

x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
===
{{
x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
x
}}
");
                var result = template.Render();
                Console.WriteLine(result);
            }
            {
                string json = @"[
          {
            'Brand': 'Nokia','Type' : 'Lumia 800',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '3.7'}
          },
          { 'Brand': 'Nokia','Type' : 'Lumia 900',
            'Specs':{'Storage' : '8GB', 'Memory': '512MB','Screensize' : '4.3' }
          },
          { 'Brand': 'HTC ','Type' : 'Titan II',
            'Specs':{'Storage' : '16GB', 'Memory': '512MB','Screensize' : '4.7' }
          }
        ]";
                var template = Template.Parse(@"
{{
x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10

x

js = " + json + @"
js

}}
");
                var result = template.Render(new { text = "Hello" });
                Console.WriteLine("\n## TestHelloWorld");
                Console.WriteLine(result);
            }
        }

        /// ////////////////////////////////////////////////////////////////////////////
    }
}
