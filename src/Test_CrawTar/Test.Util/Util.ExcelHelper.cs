using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Test.Util
{
    public class ExcelHelper
    {
        public void GenExcel(IList data, Type type, FileInfo io,string sheetName)
        {
            using (ExcelPackage excel = new ExcelPackage(io))
            {
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add(sheetName);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
                SetIntentionDataColumn(ws, type);

                for (var i = 0; i < data.Count; i++)
                {
                    for (var j = 0; j < properties.Count; j++)
                    {
                        var index = GetColumnIndex(j) + (i + 2);
                        ws.Cells[index].Value = properties[j].GetValue(data[i]);
                    }
                }

                if (ws.Dimension?.Address != null)
                {
                    //设置正文字体,字号
                    ws.Cells[ws.Dimension.Address].Style.Font.SetFromFont(new Font("Arial", 11));
                }
                excel.Save();
            }
        }

        private void SetIntentionDataColumn(ExcelWorksheet ws, Type type)
        {
            var properties = type.GetProperties();

            if (!properties.Any())
            {
                return;
            }

            for (int i = 0; i < properties.Length; i++)
            {
                var index = GetColumnIndex(i) + "1";
                //获取强类型对象中,每个属性的DisplayName特性设置的值.如有 设置成表头
                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(properties[i], typeof(DisplayNameAttribute));
                ws.Cells[index].Value = attr != null ? attr.DisplayName : properties[i].Name;
                var column = ws.Column(i + 1);
                var style = column.Style;

                //根据列的类型,设置生成Excel格式
                style.Numberformat.Format = SetNumberFormat(properties[i].PropertyType);

                //设置列自动宽度
                column.BestFit = true;
            }
        }

        private string SetNumberFormat(Type type)
        {
            string result = "@";
            if (type == typeof(string))
                result = "@";
            else if (type == typeof(sbyte))
                result = "##0";
            else if (type == typeof(byte))
                result = "##0";
            else if (type == typeof(bool))
                result = "0";
            else if (type == typeof(short))
                result = "####0";
            else if (type == typeof(ushort))
                result = "####0";
            else if (type == typeof(int) || type == typeof(int?))
                result = "#,##0";
            else if (type == typeof(uint))
                result = "#,##0";
            else if (type == typeof(long) || type == typeof(long?))
                result = "#,##0";
            else if (type == typeof(ulong))
                result = "#,##0";
            else if (type == typeof(float))
                result = ".#######";
            else if (type == typeof(double))
                result = "0.################";
            else if (type == typeof(decimal))
                result = "0.00##########################";
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
                result = "yyyy/mm/dd";

            return result;
        }

        private string GetColumnIndex(int index)
        {
            string[] letters = { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] letters2 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            return letters[index / letters2.Length] + letters2[index % letters2.Length];
        }
    }
}
