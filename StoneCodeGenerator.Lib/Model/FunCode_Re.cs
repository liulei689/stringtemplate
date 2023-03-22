using System.ComponentModel;

namespace StoneCodeGenerator.Lib.Model
{
    [Description("方法模板_默认_2")]
    public class FunCode_Defult_2
    {
        [Description("方法作用")]
        public string FUseFor { get; set; }
        [Description("函数名称")]
        public string Fname { get; set; }

        [Description("函数注释")]
        public string FCment { get; set; }
        [Description("收缩名称")]
        public string FSout { get; set; }
        [Description("模板路径")]
        public string FTmpPath { get; set; } = "模板/方法模板_默认_1.txt";
    }
}