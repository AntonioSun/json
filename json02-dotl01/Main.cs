using System;
using System.Collections.Generic;
using System.Text;
using System.IO; // TextWriter

using DotLiquid;


class ProgTest
{
    static void Main(string[] args)
    {
        
        Demo1.Demo0();
        Demo1.DemoFilter();

        // 启动时把这个过滤器注册到DotLiquid
        //Template.RegisterFilter(typeof(Demo1.DotliquidCustomFilter));
        Demo1.CustomFilter();

        Demo1.DemoTag();

        // 启动时把这个过滤器注册到DotLiquid
        //Template.RegisterTag<Demo1.ConditionalTag>("conditional");
        Demo1.DemoCustomTag();

        // 把ExampleViewModel注册为可描画的对象
        Template.RegisterSafeType(typeof(Demo1.ExampleViewModel), Hash.FromAnonymousObject);
        Demo1.DemoCustomObject();

        // 
        ToUseDL.ToTest.TestIt();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}

// DotLiquid Intro
// http://www.cnblogs.com/zkweb/p/5864794.html
public class Demo1
{
    public static void Demo0()
    {
        var template = Template.Parse("Hello, {{ name }}!");
        var result = template.Render(Hash.FromAnonymousObject(new { name = "World" }));
        Console.WriteLine(result);
    }

    public static void DemoFilter()
    {
        var template = Template.Parse("Hello, {{ name | upcase | escape }}!");
        var result = template.Render(Hash.FromAnonymousObject(new { name = "<World>" }));
        Console.WriteLine(result);
    }

    // ## Define a Filter
    // 函数名称就是过滤器名称。过滤器支持多个参数和默认参数。
    public class DotliquidCustomFilter
    {
        public static string Substr(string value, int startIndex, int length = -1)
        {
            if (length >= 0)
                return value.Substring(startIndex, length);
            return value.Substring(startIndex);
        }
    }

    public static void CustomFilter()
    {
        // 把过滤器注册到DotLiquid
        Template.RegisterFilter(typeof(DotliquidCustomFilter));

        var template = Template.Parse("Hello, {{ name | substr: 1, 3 }}!");
        var result = template.Render(Hash.FromAnonymousObject(new { name = "World" }));
        Console.WriteLine(result);
        // 显示 Hello, orl!
    }

    /*
     * ## 使用标签
        DotLiquid中有两种标签，一种是普通标签(Block)，一种是自闭合标签(Tag)。
        这里的assign是自闭合标签，if是普通标签，普通标签需要用end+标签名闭合。
    */
    public static void DemoTag()
    {
        var template = Template.Parse(@"
        {% assign name = 'World' %}
        {% if visible %}
        Hello, {{ name }}!
        {% endif %}
    ");
        var result = template.Render(Hash.FromAnonymousObject(new { visible = true }));
        Console.WriteLine("\n## Use Tag");
        Console.WriteLine(result);
        // 显示 Hello, World!
    }

    /*
     * ## 自定义标签
        这里定义一个自闭合标签conditional，这个标签有三个参数，如果第一个参数成立则描画第二个否则描画第三个参数。
     */

    public class ConditionalTag : Tag
    {
        public string ConditionExpression { get; set; }
        public string TrueExpression { get; set; }
        public string FalseExpression { get; set; }

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            var expressions = markup.Trim().Split(' ');
            ConditionExpression = expressions[0];
            TrueExpression = expressions[1];
            FalseExpression = expressions.Length >= 3 ? expressions[2] : "";
        }

        public override void Render(Context context, TextWriter result)
        {
            var condition = context[ConditionExpression];
            if (!(condition == null || condition.Equals(false) || condition.Equals("")))
                result.Write(context[TrueExpression]);
            else
                result.Write(context[FalseExpression]);
        }
    }

    public static void DemoCustomTag()
    {
        // 把过滤器注册到DotLiquid
        Template.RegisterTag<ConditionalTag>("conditional");

        var template = Template.Parse("{% conditional cond foo bar %}");
        var result = template.Render(Hash.FromAnonymousObject(new { cond = false, foo = "Foo", bar = "Bar" }));
        Console.WriteLine("\n## Custom Tag");
        Console.WriteLine(result);
        // 显示 Bar
    }

    // ## 描画自定义对象
    public class ExampleViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /*
     * DotLiquid为了安全性，默认不允许描画未经注册的对象，这样即使模板由前端使用者提供也不会导致信息泄露。
     * 为了解决上面的错误，需要把ExampleViewModel注册为可描画的对象。
     */

    public static void DemoCustomObject()
    {
        var template = Template.Parse("Name: {{ model.Name }}, Age: {{ model.Age }}");
        var model = new ExampleViewModel() { Name = "john", Age = 35 };
        var result = template.Render(Hash.FromAnonymousObject(new { model }));
        Console.WriteLine("\n## Custom Object");
        Console.WriteLine(result);
        // 显示 Name: john, Age: 35
    }

}
