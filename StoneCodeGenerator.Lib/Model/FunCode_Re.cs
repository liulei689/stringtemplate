﻿using System.ComponentModel;

namespace StoneCodeGenerator.Lib.Model
{
    [Description("代码库")]
    public class Codess
    {
        [ReadOnly(true)]
        [Description("序号")]
        public string _id { get; set; }
        [Description("语言")]
        [IsCombox(true)]
        public string Language { get; set; } = "C#";
        [Description("用处")]
        public string Use { get; set; } = "";
        [Description("用处细节")]
        public string UseDetail { get; set; } = "";
        [Description("来源")]
        [IsCombox(true)]
        public string From { get; set; } = "";
        [Description("技术")]
        [IsCombox(true)]
        public string Technical { get; set; } = "Csharp工具代码";
        [Description("更新时间")]
        [ReadOnly(true)]
        public string TimeUpate { get; set; } = "";
        [Description("访问时间")]
        [ReadOnly(true)]
        public string ReadTime { get; set; } = "";
        [Description("创建时间")]
        [ReadOnly(true)]
        public string CreateTime { get; set; } = "";
        [Description("阅读数")]
        [ReadOnly(true)]
        public int ReadCount { get; set; } = 0;
        [Description("代码")]
        public string Code { get; set; } = "";

    }
   
}