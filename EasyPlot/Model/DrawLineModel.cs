using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasyPlot.Model
{
    public class DrawLineModel
    {
        /// <summary>
        /// 起始点
        /// </summary>
        public Point StartPoint { get; set; }
        /// <summary>
        /// 终点
        /// </summary>
        public Point EndPoint { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 父级标识
        /// </summary>
        public string ParentUid { get; set; }
    }
}
