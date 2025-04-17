using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlot.Model
{
    public  class BaseClusterModel
    {
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        internal double Width { get; set; }
        internal double Height { get; set; }
        internal int Depth { get; set; }
    }
}
