using MiniSoftware;
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
  
    public class ToWordService : IToWordService
    {
        public string GetClassByExcelRowOne(string excelrowone)
        {
            var value = new Dictionary<string, object>()
            {
                ["tag"] = new MiniWordPicture() { Path = "C:\\Users\\guangbo\\Pictures\\新建文件夹\\周涛-21520601106.jpg", Width = 40, Height = 40 }
            };
            MiniSoftware.MiniWord.SaveAsByTemplate("C:\\Users\\guangbo\\Videos\\2.docx", "C:\\Users\\guangbo\\Videos\\1.docx", value);
            return "";
        }

        public string GetSting1(string excelrowone)
        {
            return "1";
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
