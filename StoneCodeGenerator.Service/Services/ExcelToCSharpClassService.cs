using StoneCodeGenerator.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Services
{
  
    public class ExcelToCSharpClassService : IExcelToCSharpClassService
    {
        public string GetClassByExcelRowOne(string excelrowone)
        {
            string text = excelrowone;
            string shouye = "\t";
            string huiche = "\n";
            string weiba = "$";
            text = Regex.Replace(text, "^\\s * (?=\\r ?$)\\n", string.Empty);
            text = Regex.Replace(text, "^", "public string? ");
            text = Regex.Replace(text, shouye, "\npublic string? ");
            text = Regex.Replace(text, huiche, " { get; set; }\n");
            text = "public class 类\n{\n" + text + "\n}";
            text = Regex.Replace(text, "^\\s * (?=\\r ?$)\\n", string.Empty);
            text = Regex.Replace(text, "\r", " ");
            text = Regex.Replace(text, "\n\n", "\n");
            return text;
        }

        public string GetDUseBy(string excelrowone)
        {
            string text = excelrowone.Replace("public string ", "a.");
            string t = "";
            string[] texts = text.Split("\r\n");
            for (int i = 0; i < texts.Length; i++) 
            {
                string str =
                   """
                   ="
                   """
                   + (i+1).ToString()
                + """
                    "; 
                    """
                   ;
              string a=  texts[i].Replace("{ get; set; }", str);
                t+=a + "\n";
            }
                return t;
        }

        public string GetSting2(string excelrowone)
        {
            return "2";
        }

        public string GetSting3(string excelrowone)
        {
            return "sdasdasdasd";
        }
    }
}
