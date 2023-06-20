using StoneCodeGenerator.Interface.DI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Interface
{
    [Description("miniexcel生成word")]
    public interface IToWordService : ISingletonDependency
    {
        [Description("替换字符")]
        /// <summary>
        /// 复制excel第一行生产类
        /// </summary>
        /// <returns></returns>
        public string GetClassByExcelRowOne(string excelrowone);
        [Description("测试1")]
        public string GetSting1(string excelrowone);
        [Description("测试2")]

        public string GetSting2(string excelrowone);
        [Description("测试3")]

        public string GetSting3(string excelrowone);

    }
}
