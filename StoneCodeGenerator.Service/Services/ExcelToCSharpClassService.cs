using StoneCodeGenerator.Service.Interface;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public string InputToOut(string excelrowone)
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
    }
}
