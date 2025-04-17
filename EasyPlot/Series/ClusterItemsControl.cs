using EasyPlot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                    child.Arrange(new Rect(x, 0, child.DesiredSize.Width, finalSize.Height));
                    x += child.DesiredSize.Width;
                    BaseClusterModel baseClusterModel = child.DataContext as BaseClusterModel;
                    if (baseClusterModel != null)
                    {
                        BaseClusters.Add(new BaseClusterModel() { Width = child.DesiredSize.Width, Height = child.DesiredSize.Height, Level = baseClusterModel.Level });
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
                        BaseClusters.Add(new BaseClusterModel() { Width = child.DesiredSize.Width, Height = child.DesiredSize.Height, Level = baseClusterModel.Level });
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
                drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = startPoint, EndPoint = endPoint, Orientation = Orientation.Horizontal });
                if (currentLevel != baseClusterModel.Level)
                {
                    if (verStartPoint.Y != verEndPoint.Y)
                    {
                        drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level - 1, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                    }
                    verStartPoint = new Point(startPoint.X, y);
                }

                verEndPoint = new Point(startPoint.X, y);
                currentLevel = baseClusterModel.Level;
                if (yIndex == BaseClusters.Count)
                {

                    if (verStartPoint.Y != verEndPoint.Y)
                    {
                        drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                    }
                }
            }
        }

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

                var startPoint = new Point(x,PreLineLength * (baseClusterModel.Level - 1));
                var endPoint = new Point(x, LineMaxSpace);
                drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = startPoint, EndPoint = endPoint, Orientation = Orientation.Horizontal });
                if (currentLevel != baseClusterModel.Level)
                {
                    if (verStartPoint.Y != verEndPoint.Y)
                    {
                        drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level - 1, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                    }
                    verStartPoint = new Point(x, startPoint.Y);
                }

                verEndPoint = new Point(x, startPoint.Y);
                currentLevel = baseClusterModel.Level;
                if (yIndex == BaseClusters.Count)
                {

                    if (verStartPoint.X != verEndPoint.X)
                    {
                        drawLines.Add(new DrawLineModel() { Level = baseClusterModel.Level, StartPoint = verStartPoint, EndPoint = verEndPoint, Orientation = Orientation.Vertical });
                    }
                }
            }
        }
        void DrawLine(DrawingContext drawingContext, List<DrawLineModel> drawLines, bool isFirst = false)
        {
            if (drawLines.Count == 0) return;
            if (isFirst)
            {
                var minLevel = drawLines.Min(x => x.Level);
                var allLineData = drawLines.FindAll(x => x.Level == minLevel);
                if (allLineData.Count > 0)
                {

                    var minData = allLineData.Min(x => x.StartPoint.Y);
                    var maxData = allLineData.Max(x => x.StartPoint.Y);
                    var maxX = allLineData.Max(x => x.StartPoint.X);
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(maxX, minData), new Point(maxX, maxData));

                }
                foreach (var draw in drawLines)
                {
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), draw.StartPoint, draw.EndPoint);
                }
            }
            drawLines = drawLines.FindAll(xx => xx.Orientation == Orientation.Vertical);
            var maxLevel = drawLines.Max(x => x.Level);
            var drawData = drawLines.Find(x => x.Level == maxLevel);
            if (maxLevel > 1)
            {
                var nextData = drawLines.Find(n => n.Level == maxLevel - 1);
                if (nextData == null) return;
                //横线
                var horEndPoint = GetPoint(drawData);

                var horStartPoint = new Point(nextData.EndPoint.X, horEndPoint.Y);

                var verStartPoint = nextData.EndPoint;

                drawingContext.DrawLine(new Pen(Brushes.Black, 1), horStartPoint, horEndPoint);
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), verStartPoint, horStartPoint);
                nextData.EndPoint = new Point(horEndPoint.X, horStartPoint.Y);
                drawLines = drawLines.FindAll(x => x.Level != maxLevel);
                DrawLine(drawingContext, drawLines);
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
    }
}
