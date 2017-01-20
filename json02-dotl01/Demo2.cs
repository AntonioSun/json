using System;
using System.Collections.Generic;

using DotLiquid;

// use DotLiquid with c#
// http://stackoverflow.com/questions/35431230/try-to-use-dotliquid-with-c-sharp

namespace ToUseDL
{

    public class User : DotLiquid.Drop
    {
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }

    // X: public class Task : DotLiquid.ILiquidizable
    // 'ToUseDL.Task' does not implement interface member 'DotLiquid.ILiquidizable.ToLiquid()'
    public class Task
    {
        public string Name { get; set; }
    }

    public class ToTest
    {
        public static void TestIt()
        {
            string myTemplate = @"
                <p>{{ user.name | upcase }} has to do:</p>
                <ul>
                {% for item in user.tasks -%}
                  <li>{{ item.name }}</li>
                {% endfor -%}
                </ul>
                ";

            var user1 = new User
            {
                Name = "Tim Jones",
                Tasks = new List<Task>  {
                    new Task { Name = "Documentation" },
                    new Task { Name = "Code comments" }
                }
            };

            var template = DotLiquid.Template.Parse(myTemplate);
            var MessageBody = "";
            //MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(user1));
                //<p> has to do:</p>
                //<ul>
                //                </ul>

            MessageBody = template.Render(DotLiquid.Hash.FromAnonymousObject(new { user = user1 }));
            //Liquid syntax error: Object 'ToUseDL.Task' is invalid because it is neither a built-in type nor implements ILiquidizable

            Console.WriteLine("\n## Using DotLiquid");
            Console.WriteLine(MessageBody);
        }
    }
}
