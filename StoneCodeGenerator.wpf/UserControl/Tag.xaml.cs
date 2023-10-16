using HandyControl.Controls;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// Tag.xaml 的交互逻辑
    /// </summary>
    public partial class Tag
    {
        public Tag()
        {
            InitializeComponent();
        }

        private void Tag_Closing(object sender, System.EventArgs e)
        {
            var d= e as HandyControl.Data.CancelRoutedEventArgs;
            d.Cancel = true;
        }
    }
}
