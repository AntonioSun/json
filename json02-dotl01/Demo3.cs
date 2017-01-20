using System;
using System.Collections.Generic;

using DotLiquid;

// use ILiquidizable 
// https://github.com/dotliquid/dotliquid/issues/32

namespace ToUseILiquidizable
{

    public class MyModel : ILiquidizable
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime StartDate { get; set; }

        public virtual object ToLiquid()
        {
            return new
            {
                FirstName,
                LastName,
                FullName = (FirstName + " " + LastName).Trim(),
                StartDate = StartDate.ToString("yyyy-MM-dd")
            };
        }
    }


    //[TestFixture]
    public class ToLiquidTest
    {
        //[Test]
        public static void ShouldUsILiquidForObjectRepresentation()
        {
            var data = new MyModel
            {
                FirstName = "Lorem",
                LastName = "ipsum",
                StartDate = DateTime.Now
            };

            var template = Template.Parse("First Name: {{ Model.FirstName }}\nLast Name: {{ Model.LastName }}\nFull Name: {{ Model.FullName }}\nDate: {{ Model.StartDate }}");

            var model = Hash.FromAnonymousObject(new { });
            model.Add("Model", Hash.FromAnonymousObject(data));

            var result = template.Render(model);
            Console.WriteLine("\n## Using DotLiquid\n### Wrong result");
            Console.WriteLine(result);
            //First Name: Lorem
            //Last Name: ipsum
            //Full Name:
            //Date: 1/20/2017 5:36:50 PM

            model = Hash.FromAnonymousObject(new { Model = data });
            result = template.Render(model);
            Console.WriteLine("\n### Correct result");
            Console.WriteLine(result);
            //First Name: Lorem
            //Last Name: ipsum
            //Full Name: Lorem ipsum
            //Date: 2017-01-20

            // result.Should().Equal("First Name: Lorem\nLast Name: ipsum\nFull Name: Lorem ipsum\nDate: " + DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}
