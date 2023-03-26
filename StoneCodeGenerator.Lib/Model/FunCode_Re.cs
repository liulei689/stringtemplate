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
        [Description("代码")]
        public string Code { get; set; } = "";
    }
}