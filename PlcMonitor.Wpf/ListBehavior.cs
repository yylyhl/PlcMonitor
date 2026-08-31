using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace PlcMonitor.Wpf
{
    public static class ListBehavior
    {
        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof(bool), typeof(ListBehavior), new PropertyMetadata(false, OnAutoScrollToEndChanged));

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value) => obj.SetValue(AutoScrollToEndProperty, value);

        public static bool GetAutoScrollToEnd(DependencyObject obj) => (bool)obj.GetValue(AutoScrollToEndProperty);

        private static void OnAutoScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsControl itemsControl && (bool)e.NewValue)
            {
                if (itemsControl.Items is INotifyCollectionChanged incc)
                {
                    incc.CollectionChanged += (s, args) =>
                    {
                        if (args.Action == NotifyCollectionChangedAction.Add && itemsControl.Items.Count > 0)
                        {
                            var last = itemsControl.Items[itemsControl.Items.Count - 1];
                            (itemsControl as ListBox)?.ScrollIntoView(last);
                            (itemsControl as ListView)?.ScrollIntoView(last);
                            if (itemsControl is System.Windows.Controls.Primitives.Selector selector)
                            {
                                var container = selector.ItemContainerGenerator.ContainerFromIndex(selector.Items.Count - 1);
                                (container as FrameworkElement)?.BringIntoView();
                            }
                        }
                    };
                }
            }
        }
    }
}
