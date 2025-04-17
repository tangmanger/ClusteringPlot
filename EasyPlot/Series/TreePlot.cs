using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasyPlot.Series
{
    public class ClusteringModel
    {
        public string Name { get; set; }
        public List<ClusteringModel> Clustering { get; set; } = new List<ClusteringModel>();
        public int Level { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Col { get; set; }
    }
    public class TreePlot : Control
    {
        public List<string> Columns
        {
            get { return (List<string>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(List<string>), typeof(TreePlot), new PropertyMetadata(null));



        public int MaxTreeLevel
        {
            get { return (int)GetValue(MaxTreeLevelProperty); }
            set { SetValue(MaxTreeLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxTreeLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTreeLevelProperty =
            DependencyProperty.Register("MaxTreeLevel", typeof(int), typeof(TreePlot), new PropertyMetadata(1));

        public double PreLevelStep { get; private set; }
        public double PreLevelHeight { get; private set; }

        private int i;

        public double MaxTextWidth { get; private set; }
        public double Levels { get; private set; }

        public int TreeDepth { get; private set; }
        public int NodeCount { get; private set; }
        public double LineMaxWidth { get; private set; }






        public bool IsVer
        {
            get { return (bool)GetValue(IsVerProperty); }
            set { SetValue(IsVerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVerProperty =
            DependencyProperty.Register("IsVer", typeof(bool), typeof(TreePlot), new PropertyMetadata(false));






        public ClusteringModel Clustering
        {
            get { return (ClusteringModel)GetValue(ClusteringProperty); }
            set { SetValue(ClusteringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clustering.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClusteringProperty =
            DependencyProperty.Register("Clustering", typeof(ClusteringModel), typeof(TreePlot), new PropertyMetadata(new PropertyChangedCallback(ClusteringCallBack)));
        private int depth;

        private static void ClusteringCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ClusteringModel clusteringModel = e.NewValue as ClusteringModel;
                TreePlot clusteringMap = d as TreePlot;
                if (clusteringMap != null)
                {
                    //计算树深
                    clusteringMap.TreeDepth = clusteringMap?.GetTreeDepth(clusteringModel) ?? 1;
                    //计算子节点数
                    clusteringMap?.ExecuteLastNode(clusteringModel);
                    //计算最大文本宽度
                    clusteringMap?.GetMaxTextWidth(clusteringModel, clusteringMap.IsVer);

                }
                clusteringMap?.InvalidateVisual();
            }
        }
        void GetMaxTextWidth(ClusteringModel root, bool isVer = false)
        {
            if (root == null) return;
            if (!string.IsNullOrWhiteSpace(root.Name))
            {
                FormattedText formattedText = DrawText(root.Name);
                double actualWidth = formattedText.WidthIncludingTrailingWhitespace;
                double actualHeight = formattedText.Height;
                if (isVer)
                {
                    MaxTextWidth = Math.Max(MaxTextWidth, actualHeight);
                }
                else
                {
                    MaxTextWidth = Math.Max(MaxTextWidth, actualWidth);
                }
            }
            if (root.Clustering != null && root.Clustering.Count > 0)
            {
                foreach (var child in root.Clustering)
                {
                    GetMaxTextWidth(child, isVer);
                }
            }
        }
        public int GetTreeDepth(ClusteringModel root)
        {
            if (root == null) return 0;

            int maxChildDepth = 0;
            if (root.Clustering != null)
            {
                foreach (var child in root.Clustering)
                {
                    int childDepth = GetTreeDepth(child);
                    maxChildDepth = Math.Max(maxChildDepth, childDepth);
                }
            }
            //root.Level = maxChildDepth + 1;
            return maxChildDepth + 1;
        }
        public void ExecuteLastNode(ClusteringModel root)
        {
            if (root != null)
            {
                if (root.Clustering != null && root.Clustering.Count > 0)
                {
                    foreach (var child in root.Clustering)
                    {
                        ExecuteLastNode(child);
                    }
                }
                else
                {
                    NodeCount++;
                }
            }

        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (IsVer)
            {

                LineMaxWidth = this.ActualHeight - this.MaxTextWidth;
                //计算每个格的宽度
                PreLevelStep = LineMaxWidth / (TreeDepth - 1);
                PreLevelHeight = this.ActualWidth / NodeCount;
                i = 0;
                depth = 0;
                DrawClustering2(Clustering, drawingContext, ActualWidth, this.ActualHeight, true);
            }
            else
            {
                //先计算还有多少可以使用
                LineMaxWidth = this.ActualWidth - this.MaxTextWidth;
                //计算每个格的宽度
                PreLevelStep = LineMaxWidth / (TreeDepth - 1);
                PreLevelHeight = this.ActualHeight / NodeCount;
                i = 0;
                depth = 0;
                DrawClustering(Clustering, drawingContext, ActualWidth, this.ActualHeight, true);
            }
            base.OnRender(drawingContext);
        }
        FormattedText DrawText(string str)
        {
            var formattedText = new FormattedText(
           str,
           CultureInfo.CurrentCulture,
             FlowDirection.LeftToRight,
           new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), 14, new SolidColorBrush((Color)Colors.Black), 1);
            return formattedText;
        }
        private Point DrawClustering2(ClusteringModel clustering, DrawingContext drawingContext, double width, double height, bool isRoot = false)
        {
            Point startPoint = new Point(0, 0);

            if (clustering != null)
            {
                if (clustering.Clustering != null && clustering.Clustering.Count > 0)
                {
                    double minX = double.MaxValue;
                    double maxX = double.MinValue;
                    double y = 0;
                    for (int i = 0; i < clustering.Clustering.Count; i++)
                    {
                        var childClustering = clustering.Clustering[i];
                        if (childClustering.Clustering != null)
                        {
                            var newPoint = DrawClustering2(childClustering, drawingContext, width, height);
                            y = newPoint.Y;
                            if (newPoint.X < minX)
                            {
                                minX = newPoint.X;
                            }
                            if (newPoint.X > maxX)
                            {
                                maxX = newPoint.X;
                            }


                        }
                    }
                    startPoint = new Point(minX, y);
                    var endPoint = new Point(maxX, y);
                    var mindX = minX + (maxX - minX) / 2;
                    //画闭合线
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                    if (!isRoot)
                    {
                        startPoint = new Point(mindX, LineMaxWidth - PreLevelStep * (TreeDepth - clustering.Level));
                        endPoint = new Point(startPoint.X, startPoint.Y + PreLevelStep);
                        drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                    }
                }
                else
                {
                    startPoint = new Point(0 + PreLevelHeight * i, LineMaxWidth - PreLevelStep * (TreeDepth - clustering.Level));
                    Point endPoint = new Point(startPoint.X, startPoint.Y + PreLevelStep);
                    if (!string.IsNullOrWhiteSpace(clustering.Name))
                    {
                        FormattedText formattedText = DrawText(clustering.Name);
                        double actualWidth = formattedText.WidthIncludingTrailingWhitespace;
                        double actualHeight = formattedText.Height;
                        Point point = new Point(0 + actualWidth / 2 + PreLevelHeight * i, height - actualHeight);
                        drawingContext.DrawText(formattedText, point);
                        startPoint.X = point.X + actualWidth / 2;
                        endPoint.Y = endPoint.Y + MaxTextWidth - actualHeight;
                        endPoint.X = startPoint.X;
                        if ((clustering.Clustering == null || clustering.Clustering.Count == 0) && clustering.Level < TreeDepth - 1)
                        {
                            var midWidth = LineMaxWidth - endPoint.Y;
                            endPoint.Y = endPoint.Y + midWidth + MaxTextWidth - actualHeight;
                        }

                    }
                    i++;
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                }
            }
            return startPoint;
        }
        private Point DrawClustering(ClusteringModel clustering, DrawingContext drawingContext, double width, double height, bool isRoot = false)
        {
            Point startPoint = new Point(0, 0);

            if (clustering != null)
            {
                if (clustering.Clustering != null && clustering.Clustering.Count > 0)
                {
                    double minY = double.MaxValue;
                    double maxY = double.MinValue;
                    double x = 0;
                    for (int i = 0; i < clustering.Clustering.Count; i++)
                    {
                        var childClustering = clustering.Clustering[i];
                        if (childClustering.Clustering != null)
                        {
                            var newPoint = DrawClustering(childClustering, drawingContext, width, height);
                            x = newPoint.X;
                            if (newPoint.Y < minY)
                            {
                                minY = newPoint.Y;
                            }
                            if (newPoint.Y > maxY)
                            {
                                maxY = newPoint.Y;
                            }


                        }
                    }
                    startPoint = new Point(x, minY);
                    var endPoint = new Point(x, maxY);
                    var mindY = minY + (maxY - minY) / 2;
                    //画闭合线
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                    if (!isRoot)
                    {
                        startPoint = new Point(LineMaxWidth - PreLevelStep * (TreeDepth - clustering.Level), mindY);
                        endPoint = new Point(startPoint.X + PreLevelStep, startPoint.Y);
                        drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                    }
                }
                else
                {
                    startPoint = new Point(LineMaxWidth - PreLevelStep * (TreeDepth - clustering.Level), 0 + PreLevelHeight * i);
                    Point endPoint = new Point(startPoint.X + PreLevelStep, startPoint.Y);
                    if (!string.IsNullOrWhiteSpace(clustering.Name))
                    {
                        FormattedText formattedText = DrawText(clustering.Name);
                        double actualWidth = formattedText.WidthIncludingTrailingWhitespace;
                        double actualHeight = formattedText.Height;
                        Point point = new Point(width - actualWidth, 0 + actualHeight / 2 + PreLevelHeight * i);
                        drawingContext.DrawText(formattedText, point);
                        startPoint.Y = point.Y + actualHeight / 2;
                        endPoint.X = endPoint.X + MaxTextWidth - actualWidth;
                        endPoint.Y = startPoint.Y;
                        if ((clustering.Clustering == null || clustering.Clustering.Count == 0) && clustering.Level < TreeDepth - 1)
                        {
                            var midWidth = LineMaxWidth - endPoint.X;
                            endPoint.X = endPoint.X + midWidth + MaxTextWidth - actualWidth;
                        }

                    }
                    i++;
                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), startPoint, endPoint);
                }
            }
            return startPoint;
        }

    }
}
