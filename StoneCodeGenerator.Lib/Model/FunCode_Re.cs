using System.ComponentModel;

namespace StoneCodeGenerator.Lib.Model
{
    [Description("代码库")]
    public class Codess
    {
        [Description("唯一标识")]
        public string _id { get; set; }
        [Description("语言")]
        public Languages Language { get; set; } = Languages.Csharp;
        [Description("用处")]
        public string Use { get; set; } = "";
        [Description("用处细节")]
        public string UseDetail { get; set; } = "";
        [Description("来源")]
        public string From { get; set; } = "";
        [Description("技术")]
        public Technicals Technical { get; set; } = Technicals.Csharp工具代码;
        [Description("更新时间")]
        public string TimeUpate { get; set; } = "";
        [Description("代码")]
        public string Code { get; set; } = "";

    }
    public enum Languages
    {
        Csharp
    }
    public enum Technicals
    {
        Csharp枚举Enum,
        CsharpLinq,
        Csharp类Class,
        Csharp继承,
        Csharp多态性,
        Csharp运算符重载,
        Csharp接口Interface,
        Csharp命名空间Namespace,
        Csharp预处理器指令,
        Csharp正则表达式,
        Csharp异常处理,
        Csharp文件的输入与输出,
        Csharp特性Attribute,
        Csharp反射Reflection,
        Csharp属性Property,
        Csharp索引器Indexer,
        Csharp委托Delegate,
        Csharp事件Event,
        Csharp集合Collection,
        Csharp泛型Generic,
        Csharp匿名方法,
        Csharp不安全代码,
        Csharp多线程,
        Csharp工具代码,
        其他
    }
}