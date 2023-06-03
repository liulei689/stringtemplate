using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using HandyControlDemo.Model;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ComboBox = HandyControl.Controls.ComboBox;

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
                contents.Children.Add(GetComboBox(model.NameDes, model.Name,model.Methods));

                //model.Methods.ForEach(method => {
                //    contents.Children.Add(GetComboBox(model.Name,method.Name, method.NameDes));
                //});
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
        private ComboBox GetComboBox(string title,string interfacename, List<PlusMethodModel> conent, bool IsEditable = false, string currentent = "")
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Name = interfacename;
            comboBox.IsEditable = IsEditable;
            comboBox.SetValue(TitleElement.TitleWidthProperty, new GridLength(100));
            comboBox.SetValue(TitleElement.TitlePlacementProperty, TitlePlacementType.Left);
            comboBox.SetValue(TitleElement.TitleProperty, title);
            // comboBox.SetValue(InfoElement.PlaceholderProperty, "L222e");
            comboBox.Margin = new Thickness(0, 5, 0, 0);

            comboBox.ItemsSource = conent;
            comboBox.DisplayMemberPath = "NameDes";
            comboBox.SelectedValuePath = "Name";
            if (IsEditable)
                comboBox.Text = currentent;
            else
                comboBox.SelectedValue = currentent;
            comboBox.SelectedIndex = 0;
            comboBox.PreviewMouseDown += PreviewMouseDownChanged;
            comboBox.SelectionChanged += Selection_Changed;
            comboBox.Foreground = new SolidColorBrush(Colors.Gray);
             comboBox.SetValue(StyleProperty, Application.Current.Resources["ComboBoxExtend"]);
            return comboBox;
        }
        private void PreviewMouseDownChanged(object sender, MouseButtonEventArgs e)
        {
        }
        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            var sc = sender as ComboBox;
            var dd = App.GetServiceByString(sc.Name);
            var select= sc.SelectedItem as PlusMethodModel;
            Type test = dd.GetType();
            MethodInfo mi1 = test.GetMethod(select.Name);
            string text = TextEditor.Text;
            object[] parameters = new object[1] { text };
            string dds = (string)mi1.Invoke(dd, parameters);
            TextEditor.Text = dds;
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
