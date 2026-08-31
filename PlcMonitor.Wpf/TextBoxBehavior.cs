using System.Windows;
using System.Windows.Controls;

namespace PlcMonitor.Wpf
{
    public static class TextBoxBehavior
    {
        #region 自动滚动到最新内容：TextBox.ScrollToEnd();
        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof(bool), typeof(TextBoxBehavior), new PropertyMetadata(false, OnAutoScrollToEndChanged));

        public static bool GetAutoScrollToEnd(DependencyObject obj) => (bool)obj.GetValue(AutoScrollToEndProperty);

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value) => obj.SetValue(AutoScrollToEndProperty, value);

        private static void OnAutoScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox && (bool)e.NewValue)
            {
                textBox.TextChanged += (s, args) =>
                {
                    textBox.ScrollToEnd();
                };
            }
        } 
        #endregion
    }
}
