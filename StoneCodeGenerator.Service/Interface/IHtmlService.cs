using StoneCodeGenerator.Interface.DI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Interface
{
    [Description("生成更新日志html")]
    public interface IHtmlService : ISingletonDependency
    {
        [Description("生成更新日志html")]
        /// <summary>
        /// 复制excel第一行生产类
        /// </summary>
        /// <returns></returns>
        public string GetClassByExcelRowOne(string excelrowone);

    }
}
