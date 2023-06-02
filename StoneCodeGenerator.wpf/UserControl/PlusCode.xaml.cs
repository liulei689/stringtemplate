using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// PropertyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class PlusCode
    {
        public PlusCode()
        {
            InitializeComponent();
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);
            TextEditor.WordWrap = true;
            PlusToUi();
        }
        private void PlusToUi()
        {
            App.plusInterfaceModels.ForEach(model => {
                model.Methods.ForEach(method => {
                    contents.Children.Add(GetButton(model.Name,method.Name, method.NameDes));
                });
            });  
        }
        private Button GetButton(string interfacename,string name, string namedes, string currentent = "")
        {
            Button button = new Button();      
            button.Name = name;
            button.Tag = interfacename;
            button.Margin = new Thickness(0, 6, 0, 0);
            button.SetValue(StyleProperty, Application.Current.Resources["ButtonDashedSuccess"]);
            button.Content = namedes;
            button.Click += Button_Click; ;
            return button;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var data= (sender as System.Windows.Controls.Button);         
           var dd=  App.GetServiceByString(data.Tag.ToString());
            Type test = dd.GetType();
            MethodInfo mi1 = test.GetMethod(data.Name);
            string text = TextEditor.Text;
            object[] parameters = new object[1] { text };
            string dds = (string)mi1.Invoke(dd, parameters);
            TextEditor.Text = dds;
        }
        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            var check =(sender as MenuItem).Header.ToString();
            if (check == "全选") TextEditor.SelectAll();
            else if(check=="复制") Clipboard.SetText(TextEditor.SelectedText);
            else if (check == "复制所有内容") Clipboard.SetText(TextEditor.Text);
        }
    }
}
