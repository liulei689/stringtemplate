using System.ComponentModel;

namespace StoneCodeGenerator.Lib.Model
{
    [Description("方法模板_异步函数默认_2")]
    public class FunCode_AsyncDefult_2
    {
        [Description("函数名称")]
        public string? Fname { get; set; }
        [Description("@方法作用")]
        public string? FUseFor { get; set; }
        [Description("函数注释")]
        public string? FCment { get; set; }
        [Description("收缩名称")]
        public string? FSout { get; set; }
        [Description("模板路径")]
        public string? FTmpPath { get; set; }= "模板/方法模板异步默认.txt";
    }
}