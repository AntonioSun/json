////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using Scriban;
using Scriban.Parsing;
//using Scriban.Runtime;

namespace Demo1
{
    class Program
    {

        // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestParser.cs
        public static void TestHelloWorld()
        {
            var template = Template.Parse(@"This is a {{ text }} World from scriban!");
            var result = template.Render(new { text = "Hello" });
            Console.WriteLine("\n## TestHelloWorld");
            Console.WriteLine(result);
            ;
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Test100expressions()
        {
            Console.WriteLine("\n## 100-expressions");
            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions
            var result = "";

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/100-assign-local-var.txt
            var template = Template.Parse(@"$x = 1
$y = 2
$x + $y
===
{{
$x = 1
$y = 2
$x + $y
}}");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/100-assign-string.txt
            template = Template.Parse(@"
### string assignment

This is a {{ x = ""string assignment"" }}but the assignment should not be outputed, only the access to the variable '{{ x }}'");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/106-parenthesis.txt
            template = Template.Parse(@"

### simple expression

(2 * (2+3))   # simple expression
===
{{
(2 * (2+3))   # simple expression
}}");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/113-unary.txt
            template = Template.Parse(@"

### 113-unary

!true -> {{ !true }}
!false -> {{ !false }}

-1 -> {{ -1 }}
-1.5 -> {{ -1.5 }}
-1000000000000 -> {{ -1000000000000 }}");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/120-array-initializer-accessor.txt
            template = Template.Parse(@"
### arrays

[1,2,3,4] -> {{ 
x = [1,2,3,4]
x[0]
x[1]
x[2]
x[3]
}}

[] -> {{ 
x = []
x[0]
x[1]
x[2]
x[3]
}}

[1,] -> {{
x = [1,]
x[0]
}}

[
1
, 2, 3, 

4] ->
{{
[
1
, 2, 3, 

4]
}}");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/130-indexer-accessor-accept1.txt
            template = Template.Parse(@"

### indexer-accessor

x = []    # x is initialized
x[0]      # accessing x with an acessor will not throw an error

y = [[""yes""]] # y is a double array
y[0][0]   # Should display ""yes""

func zzz
	ret $0[0]
end
zzz [""ok""]   # Should display ""ok""
===
{{
x = []    # x is initialized
x[0]      # accessing x with an acessor will not throw an error

y = [[""yes""]] # y is a double array
y[0][0]   # Should display ""yes""

func zzz
	ret $0[0]
end
zzz [""ok""]   # Because there is a space before an [, the parser should transform this to a function call
}}
");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/131-member-accessor-accept1.txt
            template = Template.Parse(@"

### member-accessor

x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
===
{{
x = { z: 10 }    # x is initialized
x.y              # x.y will work
x.z              # x.z should print 10
}}
");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/140-object-initializer-accessor.txt
            template = Template.Parse(@"

### object

{member1: 1, member2: 2, member3: 3, member4: 4} -> {{ x = {member1: 1, member2: 2, member3: 3, member4: 4}
x.member1
x.member2
x.member3
x.member4
}}

{member1: 1, } -> {{ x = {member1: 1, }
x.member1
}}

{member1: [""a"", ""b"", ""c""], } -> {{ x = {member1: [""a"", ""b"", ""c""], }
x.member1[0]
x.member1[1]
x.member1[2]
}}

{member1: {submember: ""yes""} } -> {{ x = {member1: {submember: ""yes""} }
x.member1.submember
}}

array access on object:
x = {member1: {submember: ""no""} } 
x[""member1""][""submember""] -> {{ x = {member1: {submember: ""no""} }
x[""member1""][""submember""]
}}

#### Test multilines

x = {
member1: 1, member2:
3,
member3: 3
,
member4: 4} ->
{{
x = {
member1: 1, member2:
2,
member3: 3
,
member4: 4}
x.member1
x.member2
x.member3
x.member4
}}

#### Test object initializer with string members instead of variable name (json like):

{{
myobject = {
        ""test"": 1,
        ""test2"" : 2
}
myobject.test + myobject.test2
}}

");
            result = template.Render();
            Console.WriteLine(result);

/*

{member1: 1, member2: 2, member3: 3, member4: 4} -> 1234

{member1: 1, } -> 1

{member1: ["a", "b", "c"], } -> abc

{member1: {submember: "yes"} } -> yes

array access on object:
x = {member1: {submember: "no"} } 
x["member1"]["submember"] -> no

#### Test multilines

x = {
member1: 1, member2:
3,
member3: 3
,
member4: 4} ->
1234

#### Test object initializer ... like):

3
*/

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/150-range-expression.txt
            template = Template.Parse(@"

### range-expression

1..5
===
{{
1..5
}}

x = 5
1..x
===
{{
x = 5
1..x
}}

(2-1)..(4+1)
===
{{
(2-1)..(4+1)
}}

1..<5
===
{{
1..<5
}}
");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/160-array-expression.txt
            template = Template.Parse(@"

### array-expression

x1 = []
x1 << 1
===
{{
x1 = []
x1 << 1
}}

x2 = []
x2 << 1
x2 << 2
===
{{
x2 = []
x2 << 1
x2 << 2
}}

x3 = []
1 >> x3
===
{{
x3 = []
1 >> x3
}}

x4 = []
1 >> x4
2 >> x4
===
{{
x4 = []
1 >> x4
2 >> x4
}}

Check precedence
x5 = []
x5 << 1 + 2
===
{{
x5 = []
x5 << 1 + 2
}}
");
            result = template.Render();
            Console.WriteLine(result);

            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/100-expressions/170-array-members.txt
            template = Template.Parse(@"

### array-members

# Show hybrid array/object usage of an array with properties
x1 = []
x1.test = 5
x1[0] = x1.test
x1
===
{{
# Show hybrid array/object usage of an array with properties
x1 = []
x1.test = 5
x1[0] = x1.test
x1
}}
");
            result = template.Render();
            Console.WriteLine(result);
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Test200statements()
        {
            Console.WriteLine("\n## 200-statements");
            // https://github.com/lunet-io/scriban/tree/master/src/Scriban.Tests/TestFiles/200-statements
            var result = "";

            var template = Template.Parse(@"
### Evaluate if statements

if true 
  ""Yes""
end
===
{{
if true 
  ""Yes""
end
}}

if false
  ""Boo!""
else
  ""Yes""
end
===
{{
if false
  ""Boo!""
else
  ""Yes""
end
}}

if false
  ""Boo!""
else if false
  ""Boo2""
else if null
  ""Boo3""
else
  ""Yes""
end
===
{{
if false
  ""Boo!""
else if false
  ""Boo2""
else if null
  ""Boo3""
else
  ""Yes""
end
}}

# Test ; as an end of statement
if false; ""Boo!""; else if false; ""Boo2""; else if null; ""Boo3""; else; ""Yes""; end
===
{{if false; ""Boo!""; else if false; ""Boo2""; else if null; ""Boo3""; else; ""Yes""; end}}

");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate for statements

x = [1,2,3,4]
for y in x
        y
end
===
{{
x = [1,2,3,4]
for y in x
        y
end
}}

x = [1,2,3,4]
for y in x
        if y == 1
                continue
        else if y == 4
                break
        end
        y
end
===
{{
x = [1,2,3,4]
for y in x
        if y == 1
                continue
        else if y == 4
                break
        end
        y
end
}}

for y in [1,2,3,4]
        (y-1) + "" => ["" + for.index + ""] "" + for.first + "","" + for.last + "","" + for.even + "","" + for.odd + ""\n""
end
===
{{
for y in [1,2,3,4]
        (y-1) + "" => ["" + for.index + ""] "" + for.first + "","" + for.last + "","" + for.even + "","" + for.odd + ""\n""
end
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate for with range statements

# inclusive range [1,2,3,4]
for i in 1..4
	i
end
===
{{
for i in 1..4
	i
end
}}

# exclude range [1,2,3]
for i in 1..<4
	i
end
===
{{
for i in 1..<4
	i
end
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"

### Evaluate <while> statement

while x < 9
        x = x + 1
        x
end
===
{{
while x < 9
        x = x + 1
        x
end
}}

while true
        x = x + 1
        if x > 9
                break
        end
        x
end
===
{{
x = 0
while true
        x = x + 1
        if x > 9
                break
        end
        x
end
}}

while x < 9
        x = x + 1
        if (x % 2) == 0
                continue
        end
        # Display only odd numbers
        x
end
===
{{
x = 0
while x < 9
        x = x + 1
        if (x % 2) == 0
                continue
        end
        # Display only odd numbers
        x
end
}}

while false
        ""No""
end
===
{{
while false
        ""No""
end
}}

x = 0
while x < 4
        x = x + 1
        (x-1) + "" => ["" + while.index + ""] "" + while.first + "","" + while.even + "","" + while.odd + ""\n""
end
===
{{
x = 0
while x < 4
        x = x + 1
        (x-1) + "" => ["" + while.index + ""] "" + while.first + "","" + while.even + "","" + while.odd + ""\n""
end
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate <capture> statement

{{y = 150}}{{capture x}}This is the text captured with the value {{y}}{{end}}{{ ""This is the capture = "" + x}}
{{x={}}}{{capture x.y}}This is the text captured with the value {{y}}{{end}}{{ ""This is the capture = "" + x.y}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate <import> statement

{{
x = {member1: ""This is the member1"", member2: 150 }
import x
member1 + "" "" + member2
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate <import> and <readonly> statement

{{
member1 = ""This is a readonly variable""
readonly member1
x = {member1: ""This is the member1"", member2: 150 }
import x
member1 + "" "" + member2
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### Evaluate <with> statement

root = {}
x = 10
with root
        member1 = ""This is a member""
        memberTemp = 5
        member2 = x + memberTemp + 1
end
if member1
        ""Unexpected member1 should not appear at global scope""
end
root.member1 + "" "" + root.member2
===
{{
root = {}
x = 10
with root
        member1 = ""This is a member""
        memberTemp = 5
        member2 = x + memberTemp + 1
end
if member1
        ""Unexpected member1 should not appear at global scope""
end
root.member1 + "" "" + root.member2
}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### ret-statement

This is a text
{{~  ret ~}}
This text will not appear");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"
### ret-statement 2

{{
func test
        ""This text is inside the function""
        ret
        ""This text is inside the function but should not be executed""
end
test
}}
This text should also appear outside of the function
");
            result = template.Render();
            Console.WriteLine(result);
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Test400builtins()
        {
            Console.WriteLine("\n## 400-builtins");
            // https://github.com/lunet-io/scriban/blob/master/src/Scriban.Tests/TestFiles/400-builtins/400-builtins.txt
            var result = "";

            var template = Template.Parse(@"
### List of all the builtin <array> functions:

{{
func dump_members
	for member in $0 | array.map 'key' | array.sort
		"" "" + member + ""\n""
	end
end~}}
{{dump_members array}}
List of all the builtin <math> functions:
{{dump_members math}}
List of all the builtin <string> functions:
{{dump_members string}}
List of all the builtin <date> functions:
{{dump_members date}}
List of all the builtin <timespan> functions:
{{dump_members timespan}}
List of all the builtin <object> functions:
{{dump_members object}}
");
            result = template.Render();
            Console.WriteLine(result);

            template = Template.Parse(@"

### 410-array

{{[1,2,3,4] | array.first}}
{{[1,2,3,4] | array.last}}
{{[1,2,3,4] | array.join ' | '}}
{{[1,5,7,9] | array.size}}
{{[1,1,2,2,3] | array.uniq}}
{{[9,3,5,1] | array.sort}}
{{[1,2,3,4] | array.add 5}}
{{[1,2,3,4] | array.add_range [5,6,7,8] }}
{{[1,2,3,4] | array.remove_at 0 | array.add 5}}
{{[1,2,3,4] | array.insert_at 1 9}}
{{[1,2,3,4] | array.reverse }}
{{[1,2,3,4] | array.remove_at (-1) # use parenthesis to avoid confusion with binary operator - }}
{{1..5 | array.first}}
{{1..5 | array.last}}
{{1..5 | array.join ' | '}}
{{2..4 | array.size}}
{{4..1 | array.sort}}
");
            result = template.Render();
            Console.WriteLine(result);
/*
1
4
1 | 2 | 3 | 4
4
[1, 2, 3]
[1, 3, 5, 9]
[1, 2, 3, 4, 5]
[1, 2, 3, 4, 5, 6, 7, 8]
[2, 3, 4, 5]
[1, 9, 2, 3, 4]
[4, 3, 2, 1]
[1, 2, 3]
1
5
1 | 2 | 3 | 4 | 5
3
[1, 2, 3, 4]
*/

            template = Template.Parse(@"
### 430-string

{{ ""test"" | string.capitalize }}
{{ ""TeSt"" | string.downcase }}
{{ ""This & is a @@^^&%%%%% value"" | string.handleize }}
{{ ""    test"" | string.lstrip }}
{{ 5 | string.pluralize 'item' 'items' }}
{{ ""This is a test with a test"" | string.remove 'test' }}
{{ ""This is a test with a test"" | string.remove_first 'test' }}
{{ ""This is a test with a test"" | string.replace 'test' 'boom'}}
{{ ""This is a test with a test"" | string.replace_first 'test' 'boom'}}
{{ ""test     "" | string.rstrip }}
{{ ""test"" | string.slice 1 }}
{{ ""test"" | string.slice (-2) }}
{{ ""a/b/c/d/e//f"" | string.split '/' | array.join ' | ' }}
{{ ""test"" | string.starts_with 'test'}}
{{ ""test"" | string.starts_with 'toto'}}
{{ ""   test   "" | string.strip }}
{{ ""test\r\ntest\r\n"" | string.strip_newlines }}
{{ ""This is a long test with several chars in it but going to be truncated"" | string.truncate 15 }}
{{ ""This is a test truncated at 5"" | string.truncatewords 5 }}
{{ ""test"" | string.upcase }}
{{ ""This will capitalize words"" | string.capitalizewords }}
");
            result = template.Render();
            Console.WriteLine(result);
/*
Test
test
This-is-a-value
test
items
This is a  with a
This is a  with a test
This is a boom with a boom
This is a boom with a test
test
est
st
a | b | c | d | e | f
true
false
test
testtest
This is a lo...
This is a test truncated...
TEST
This Will Capitalize Words
*/

            template = Template.Parse(@"
### 440-date

{{ date.parse '2016/01/05' | date.to_string '%g' }}
{{ date.parse '2016/01/05' | date.add_days 1 | date.to_string '%g' }}
{{ date.parse '2016/01/05' | date.add_months 1 | date.to_string '%g' }}
{{ date.parse '2016/01/05' | date.add_years 1 | date.to_string '%g' }}
{{ x = date.parse '2016/01/05' ~}}
{{ x.year }}
{{ x.month }}
{{ x.day }}
");
            result = template.Render();
            Console.WriteLine(result);
/*
 05 Jan 2016
 06 Jan 2016
 05 Feb 2016
 05 Jan 2017
2016
1
5
*/

            template = Template.Parse(@"
### 450-object

{{ null | object.typeof }}
{{ true | object.typeof }}
{{ 1 | object.typeof }}
{{ 1.0 | object.typeof }}
{{ ""text"" | object.typeof }}
{{ 1..5 | object.typeof }}
{{ [1,2,3,4,5] | object.typeof }}
{{ {} | object.typeof }}
{{ object | object.typeof }}

{{ { a: 1, b: 2} | object.values }}
{{ { a: 1, b: 2} | object.keys }}
");
            result = template.Render();
            Console.WriteLine(result);
/*

boolean
number
number
string
iterator
array
object
object

[1, 2]
[a, b]
*/
        }
    }
}
