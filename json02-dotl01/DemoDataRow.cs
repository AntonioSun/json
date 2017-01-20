using System;
using System.Data;
using System.Collections.Generic;

using DotLiquid;

// Wrapping the DataRow object
// http://stackoverflow.com/questions/4424341/
// https://github.com/dotliquid/dotliquid/blob/master/src/DotLiquid.Tests/DropTests.cs#L284

namespace ToTestDataRow
{
    internal class DataRowDrop : Drop
    {
        private readonly System.Data.DataRow _dataRow;

        public DataRowDrop(System.Data.DataRow dataRow)
        {
            _dataRow = dataRow;
        }

        public override object BeforeMethod(string method)
        {
            if (_dataRow.Table.Columns.Contains(method))
                return _dataRow[method];
            return null;
        }
    }

    public class ToDRTest
    {
        public static void TestDataRowDrop()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Column1");
            dataTable.Columns.Add("Column2");

            System.Data.DataRow dataRow = dataTable.NewRow();
            dataRow["Column1"] = "Hello";
            dataRow["Column2"] = "World";

            Template tpl = Template.Parse(" {{ row.column1 }}, ~{{ row.column2 }}~! ");
            var result = tpl.Render(Hash.FromAnonymousObject(new { row = new DataRowDrop(dataRow) }));
            Console.WriteLine("\n## Using DataRow");
            Console.WriteLine(result);
            //  Hello, ~World~!
        }
    }
}
