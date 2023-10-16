using DocumentFormat.OpenXml.Spreadsheet;
using JinianNet.JNTemplate;
using StoneCodeGenerator.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Services
{
  
    public class HtmlService : IHtmlService
    {
        public string GetClassByExcelRowOne(string excelrowone)
        {
            var ls= excelrowone.Split(Environment.NewLine);

            Engine.UseInterpretationEngine();
            Engine.Configure((c) =>
            {
                c.OutMode = OutMode.Auto;
            });
            // ls= File.ReadAllLines("C:\\Users\\guangbo\\Desktop\\12345.txt");
            var datarender = new List<list>() { };
            foreach (var line in ls)
            {
                if(string.IsNullOrEmpty(line)) continue;
             var lines=   line.Split('\t');
                if (lines.Length != 4) continue;
                if (DateTime.TryParse(lines[2], out DateTime time))
                {

                    var lists = new list();
                    lists.YearMon = time.ToString("yyyy年MM月", new CultureInfo("zh-CN"));
                    lists.day = time.Day + "日";
                    if (lines[3].Contains("v1.0.0."))
                    {
                        var data = lines[3].Split("v1.0.0.");
                        lists.version = "v1.0.0." + data[1];
                    }
                    else lists.version = "";
                    lists.detail = "版本更新";
                    var linescontent = lines[3].Split(new char[] { '，', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);

                    var detailss = new List<string>() { };
                    for (int i = 0; i < linescontent.Length; i++)
                    {
                        detailss.Add((i + 1) + "." + linescontent[i]);
                    }

                    lists.details = detailss;

                    datarender.Add(lists);
                }
          
            }

            var template = Engine.LoadTemplate(AppDomain.CurrentDomain.BaseDirectory+ "模板\\logs.html");
            template.Set("list", datarender);
            var result = template.Render();
            return result;
        }

  
    }
    public class list 
    {
      public string YearMon { get; set; }
      public string day { get; set; }
        public string version { get; set; }
        public string detail { get; set; }
        public List<string> details { get; set; }
    }
   
}
