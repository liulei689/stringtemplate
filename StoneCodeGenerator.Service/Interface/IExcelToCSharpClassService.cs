using StoneCodeGenerator.Service.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Interface
{
    public interface IExcelToCSharpClassService:ISingletonDependency
    {
        /// <summary>
        /// 复制excel第一行生产类
        /// </summary>
        /// <returns></returns>
        public string GetClassByExcelRowOne(string excelrowone);
    }
}
