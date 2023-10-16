using StoneCodeGenerator.Interface.DI;
using System.ComponentModel;
using System.Globalization;

namespace StoneCodeGenerator1.TestService1
{
    [Description("生成更新日志html")]
    public interface ITestService1 : ISingletonDependency
    {
        [Description("测试")]
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public string GetClassByExcelRowOne(string excelrowone);

    }

    public class HtmlService21 : ITestService1
    {
        public string GetClassByExcelRowOne(string excelrowone)
        {
           
            return excelrowone+"343434";
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