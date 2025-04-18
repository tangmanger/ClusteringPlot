using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyPlot.Behaviors
{
    public class NoticeSizeChangedBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var element = (AssociatedObject as FrameworkElement);
            if (element != null)
                element.SizeChanged += Element_SizeChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var element = (AssociatedObject as FrameworkElement);
            if (element != null)
                element.SizeChanged -= Element_SizeChanged;

        }


        // Using a DependencyProperty as the backing store for OvservedWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OvservedWidthProperty =
            DependencyProperty.Register("OvservedWidth", typeof(double), typeof(NoticeSizeChangedBehavior), new PropertyMetadata(0d));

        public static readonly DependencyProperty OvseredHeightProperty =
          DependencyProperty.Register("OvseredHeight", typeof(double), typeof(NoticeSizeChangedBehavior), new PropertyMetadata(0d));
        public double OvseredHeight
        {
            get { return (double)GetValue(OvseredHeightProperty); }
            set { SetValue(OvseredHeightProperty, value); }
        }
        public double OvservedWidth
        {
            get { return (double)GetValue(OvservedWidthProperty); }
            set { SetValue(OvservedWidthProperty, value); }
        }
        private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OvservedWidth = (sender as FrameworkElement).ActualWidth;
            OvseredHeight = (sender as FrameworkElement).ActualHeight;
        }
    }
}
