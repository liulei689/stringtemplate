﻿using StoneCodeGenerator.Interface.DI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.Interface
{
    [Description("excel生成c#代码")]
    public interface IExcelToCSharpClassService:ISingletonDependency
    {
        [Description("表格第一行生成c#实体")]
        /// <summary>
        /// 复制excel第一行生产类
        /// </summary>
        /// <returns></returns>
        public string GetClassByExcelRowOne(string excelrowone);
        [Description("根据类生成对象调用")]
        public string GetDUseBy(string excelrowone);
        [Description("测试2")]

        public string GetSting2(string excelrowone);
        [Description("测试3")]

        public string GetSting3(string excelrowone);

    }
}
