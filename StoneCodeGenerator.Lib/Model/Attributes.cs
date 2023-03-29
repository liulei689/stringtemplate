using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Lib.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsCombox : Attribute
    {
        private bool canuse;
        /// <summary>
        /// 是否下拉框
        /// </summary>
        /// <param name="是否可用"></param>
        public IsCombox(bool 是否可用)
        {
            canuse = 是否可用;
        }

        public bool Canuse
        {
            get
            {
                return canuse;
            }
        }

    }
    //[AttributeUsage(AttributeTargets.Property)]
    //public class ReadOnly : Attribute
    //{
    //    private bool canuse;
    //    /// <summary>
    //    /// 是否下拉框
    //    /// </summary>
    //    /// <param name="是否可用"></param>
    //    public ReadOnly(bool 是否可用)
    //    {
    //        canuse = 是否可用;
    //    }

    //    public bool Canuse
    //    {
    //        get
    //        {
    //            return canuse;
    //        }
    //    }

    //}
}
