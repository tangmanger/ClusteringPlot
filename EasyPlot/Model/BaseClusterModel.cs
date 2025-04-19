using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlot.Model
{
    /// <summary>
    /// 基础分类
    /// </summary>
    public class BaseClusterModel
    {
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 每个元素宽度
        /// </summary>
        internal double Width { get; set; }
        /// <summary>
        /// 每个元素高度
        /// </summary>
        internal double Height { get; set; }
        /// <summary>
        /// 父级唯一标识
        /// </summary>
        public string ParentUid { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Uid { get; set; }

    }
}
