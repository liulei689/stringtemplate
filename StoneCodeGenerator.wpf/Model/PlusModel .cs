using HandyControl.Data;
using System.Collections.Generic;

namespace HandyControlDemo.Model
{
    /// <summary>
    /// 扩展接口-存储扩展功能基本信息
    /// </summary>
    public class PlusInterfaceModel: PlusBaseModel
    {
        /// <summary>
        /// 方法
        /// </summary>
        public List<PlusMethodModel> Methods { get; set; }
    }
    /// <summary>
    /// 扩展方法功能基本信息
    /// </summary>
    public class PlusMethodModel : PlusBaseModel
    {

    }
    public class PlusBaseModel 
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 作用显示文本
        /// </summary>
        public string NameDes { get; set; }
    }
}
