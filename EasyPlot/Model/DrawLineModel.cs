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
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Orientation Orientation { get; set; }
        public int Level { get; set; }
        public string Uid { get; set; }
        public string ParentUid { get; set; }
    }
}
