using System.ComponentModel;

namespace StoneCodeGenerator.Lib.Model
{
    [Description("代码库")]
    public class Codess
    {
        [Description("唯一标识")]
        public string _id { get; set; }
        [Description("语言")]
        public string Language { get; set; } = "C#";
        [Description("用处")]
        public string Use { get; set; } = "";
        [Description("用处细节")]
        public string UseDetail { get; set; } = "";
        [Description("来源")]
        public string From { get; set; } = "";
        [Description("技术")]
        public Technicals Technical { get; set; } = Technicals.Csharip技术;
        [Description("更新时间")]
        public string TimeUpate { get; set; } = "";
        [Description("代码")]
        public string Code { get; set; } = "";
    }
    public enum Technicals { 
    Csharip技术,
    Linq,
    反射,
    }
}