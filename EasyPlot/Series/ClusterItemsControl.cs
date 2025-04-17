using EasyPlot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EasyPlot.Series
{
    internal class ClusterItemsControl : ItemsControl
    {
    }
    public class ClusterPanel : Panel
    {

        public int MaxLevel
        {
            get { return (int)GetValue(MaxLevelProperty); }
            set { SetValue(MaxLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLevelProperty =
            DependencyProperty.Register("MaxLevel", typeof(int), typeof(ClusterPanel), new PropertyMetadata(0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ClusterPanel), new PropertyMetadata(Orientation.Horizontal));


        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size();
            if (Orientation == Orientation.Horizontal)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);
                    size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                    size.Width += child.DesiredSize.Width;
                }
            }
            else
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);
                    size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                    size.Height += child.DesiredSize.Height;
                }
            }
            return size;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            BaseClusters = new List<BaseClusterModel>();
            if (Orientation == Orientation.Horizontal)
            {
                double x = 0;
                foreach (FrameworkElement child in InternalChildren)
                {
                    child.Arrange(new Rect(x, ActualHeight - child.DesiredSize.Height, child.DesiredSize.Width, child.DesiredSize.Height));
                    x += child.DesiredSize.Width;
                    BaseClusterModel baseClusterModel = child.DataContext as BaseClusterModel;
                    if (baseClusterModel != null)
                    {
                        BaseClusters.Add(new BaseClusterModel() { Width = child.DesiredSize.Width, Height = child.DesiredSize.Height, Uid = baseClusterModel.Uid, ParentUid = baseClusterModel.ParentUid, Level = baseClusterModel.Level });
                    }
                }
                if (InternalChildren.Count > 0)
                    LineMaxSpace = this.ActualHeight - InternalChildren[0].DesiredSize.Height;
            }
            else
            {
                double y = 0;
                foreach (FrameworkElement child in InternalChildren)
                {
                    child.Arrange(new Rect(ActualWidth - child.DesiredSize.Width, y, child.DesiredSize.Width, child.DesiredSize.Height));
                    y += child.DesiredSize.Height;
                    BaseClusterModel baseClusterModel = child.DataContext as BaseClusterModel;
                    if (baseClusterModel != null)
                    {
                        BaseClusters.Add(new BaseClusterModel() { Width = child.DesiredSize.Width, Height = child.DesiredSize.Height, Uid = baseClusterModel.Uid, ParentUid = baseClusterModel.ParentUid, Level = baseClusterModel.Level });
                    }
                }
                if (InternalChildren.Count > 0)
                    LineMaxSpace = this.ActualWidth - InternalChildren[0].DesiredSize.Width;
            }
            return finalSize;
        }


        private List<BaseClusterModel> BaseClusters { get; set; }
        public double LineMaxSpace { get; private set; }
        public double PreLineLength { get; private set; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
            //计算每个格的宽度
            PreLineLength = LineMaxSpace / (MaxLevel);
            List<DrawLineModel> drawLines = new List<DrawLineModel>();


            int yIndex = 0;
            double y = 0;
            int currentLevel = 0;
            var verStartPoint = new Point(0, 0);
            var verEndPoint = new Point(0, 0);
            //绘制横线
            if (Orientation == Orientation.Vertical)
                DrawVer(drawLines, ref yIndex, ref y, ref currentLevel, ref verStartPoint, ref verEndPoint);
            else
                DrawHor(drawLines, ref yIndex, ref y, ref currentLevel, ref verStartPoint, ref verEndPoint);

            DrawLine(drawingContext, drawLines, true);

            base.OnRender(drawingContext);
        }

        private void DrawVer(List<DrawLineModel> drawLines, ref int yIndex, ref double y, ref int currentLevel, ref Point verStartPoint, ref Point verEndPoint)
        {
            foreach (BaseClusterModel baseClusterModel in BaseClusters)
            {

                if (yIndex == 0)
                {
                    y = baseClusterModel.Height / 2;
                }
                else
                {
                    y += baseClusterModel.Height;
                }
                yIndex++;

                var startPoint = new Point(PreLineLength * (baseClusterModel.Level - 1), y);
                var endPoint = new Point(LineMaxSpace, y);
                drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, Uid = baseClusterModel.Uid, ParentUid = baseClusterModel.ParentUid, StartPoint = startPoint, EndPoint = endPoint, Orientation = Orientation.Horizontal });
                if (currentLevel != baseClusterModel.Level)
                {
                    //if (verStartPoint.Y != verEndPoint.Y)
                    //{
                    //    drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level - 1, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                    //}
                    verStartPoint = new Point(startPoint.X, y);
                }

                verEndPoint = new Point(startPoint.X, y);
                currentLevel = baseClusterModel.Level;
                //if (yIndex == BaseClusters.Count)
                //{

                //    if (verStartPoint.Y != verEndPoint.Y)
                //    {
                //        drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                //    }
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawLines"></param>
        /// <param name="yIndex"></param>
        /// <param name="x"></param>
        /// <param name="currentLevel"></param>
        /// <param name="verStartPoint"></param>
        /// <param name="verEndPoint"></param>
        private void DrawHor(List<DrawLineModel> drawLines, ref int yIndex, ref double x, ref int currentLevel, ref Point verStartPoint, ref Point verEndPoint)
        {
            foreach (BaseClusterModel baseClusterModel in BaseClusters)
            {

                if (yIndex == 0)
                {
                    x = baseClusterModel.Width / 2;
                }
                else
                {
                    x += baseClusterModel.Width;
                }
                yIndex++;

                var startPoint = new Point(x, PreLineLength * (baseClusterModel.Level - 1));
                var endPoint = new Point(x, LineMaxSpace);
                drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = startPoint, Uid = baseClusterModel.Uid, ParentUid = baseClusterModel.ParentUid, EndPoint = endPoint, Orientation = Orientation.Horizontal });

                verEndPoint = new Point(x, startPoint.Y);
                currentLevel = baseClusterModel.Level;

            }
        }


        void DrawByLevel(DrawingContext drawingContext, List<DrawLineModel> drawLines, int level)
        {
            var allLevelData = drawLines.FindAll(x => x.Level == level);
            if (allLevelData != null && allLevelData.Count > 0)
            {
                var uidDic = allLevelData.GroupBy(x => x.ParentUid).ToDictionary(c => c.Key, m => m.ToList());
                foreach (var item in uidDic)
                {
                    var parentLine = drawLines.Find(p => p.Uid == item.Key);
                    if (item.Value.Count == 1)
                    {
                        if (parentLine != null)
                        {
                            var startPoint = new Point(item.Value[0].StartPoint.X, item.Value[0].StartPoint.Y);
                            var endPoint = new Point(item.Value[0].StartPoint.X, parentLine.EndPoint.Y);
                            drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                        }
                    }
                    else
                    {
                        var minData = item.Value.Min(x => x.StartPoint.Y);
                        var maxData = item.Value.Max(x => x.StartPoint.Y);
                        var maxX = item.Value.Max(x => x.StartPoint.X);
                        drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(maxX, minData), new Point(maxX, maxData));
                        if (parentLine != null)
                        {
                            var horEndPoint = GetPoint(maxX, minData, maxData);
                            var horStartPoint = new Point(parentLine.StartPoint.X, horEndPoint.Y);
                            drawingContext.DrawLine(new Pen(Brushes.Black, 1), horStartPoint, horEndPoint);
                            var newDrawLine = new DrawLineModel() { ParentUid = parentLine.ParentUid, Level = parentLine.Level, Uid = Guid.NewGuid().ToString(), StartPoint = horStartPoint, EndPoint = horEndPoint };
                            drawLines.Add(newDrawLine);
                        }
                    }
                }
                level = level - 1;
                DrawByLevel(drawingContext, drawLines, level);
            }
        }
        private void DrawHorLevel(DrawingContext drawingContext, List<DrawLineModel> drawLines, int level)
        {
            var allLevelData = drawLines.FindAll(x => x.Level == level);
            if (allLevelData != null && allLevelData.Count > 0)
            {
                var uidDic = allLevelData.GroupBy(x => x.ParentUid).ToDictionary(c => c.Key, m => m.ToList());
                foreach (var item in uidDic)
                {
                    var parentLine = drawLines.Find(p => p.Uid == item.Key);
                    if (item.Value.Count == 1)
                    {
                        if (parentLine != null)
                        {
                            var startPoint = new Point(item.Value[0].StartPoint.X, item.Value[0].StartPoint.Y);
                            var endPoint = new Point(item.Value[0].StartPoint.X, parentLine.EndPoint.Y);
                            drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                        }
                    }
                    else
                    {
                        var minData = item.Value.Min(x => x.StartPoint.X);
                        var maxData = item.Value.Max(x => x.StartPoint.X);
                        var maxY = item.Value.Max(x => x.StartPoint.Y);
                        drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(minData, maxY), new Point(maxData, maxY));
                        if (parentLine != null)
                        {
                            var verEndPoint = GetPointY(maxY, minData, maxData);
                            var verStartPoint = new Point(verEndPoint.X, parentLine.StartPoint.Y);
                            drawingContext.DrawLine(new Pen(Brushes.Black, 1), verStartPoint, verEndPoint);
                            var newDrawLine = new DrawLineModel() { ParentUid = parentLine.ParentUid, Level = parentLine.Level, Uid = Guid.NewGuid().ToString(), StartPoint = verStartPoint, EndPoint = verEndPoint };
                            drawLines.Add(newDrawLine);
                        }
                    }
                }
                level = level - 1;
                DrawHorLevel(drawingContext, drawLines, level);
            }
        }
        void DrawLine(DrawingContext drawingContext, List<DrawLineModel> drawLines, bool isFirst = false)
        {
            if (drawLines.Count == 0) return;
            if (isFirst)
            {
                foreach (var draw in drawLines)
                {
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), draw.StartPoint, draw.EndPoint);
                }
                if (Orientation == Orientation.Vertical)
                    DrawByLevel(drawingContext, drawLines, MaxLevel);
                else
                    DrawHorLevel(drawingContext, drawLines, MaxLevel);
                return;

            }


        }

      

        private Point GetPoint(DrawLineModel drawData)
        {
            var distance = drawData.EndPoint.Y - drawData.StartPoint.Y;
            var y = Math.Max(drawData.EndPoint.Y, drawData.StartPoint.Y);
            y = (y - Math.Abs(distance) / 2);
            var x = drawData.StartPoint.X;
            return new Point(x, y);
        }
        private Point GetPoint(double x, double minY, double maxY)
        {
            var distance = maxY - minY;
            var y = Math.Max(maxY, minY);
            y = (y - Math.Abs(distance) / 2);
            return new Point(x, y);
        }
        private Point GetPointY(double y, double minX, double maxX)
        {
            var distance = maxX - minX;
            var x = Math.Max(maxX, maxX);
            x = (x - Math.Abs(distance) / 2);
            return new Point(x, y);
        }
    }
}
