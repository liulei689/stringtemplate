using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using StoneCodeGenerator.Lib;
using StoneCodeGenerator.Lib.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ICSharpCode.AvalonEdit.Document.TextDocumentWeakEventManager;
using static System.Net.Mime.MediaTypeNames;
using TextBox = HandyControl.Controls.TextBox;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// PropertyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class Codes
    {

        public Codes()
        {
            InitializeComponent();
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);
            templist.ItemsSource = new string[] { "1", "2" };
            templist.SelectedIndex = 0;
            templist_content.ItemsSource = new string[] { "1", "2" };
            templist_content.SelectedIndex = 0;
            TextEditor.WordWrap = true; 
            CreateForm(new Codess());
            _o = new Codess();
        }
        private void templist_Selected(object sender, RoutedEventArgs e)
        {
            if (templist.SelectedValue != null)
            {
                var ddsdsadd21 = new Litedb().Selects();
                var ddd = templist.SelectedValue.ToString();
                var ddsdsadd = new Litedb().Selects().Find(o => o.Use == ddd);

                if(ddsdsadd!=null)
                TextEditor.Text = ddsdsadd.Code;
                //templist_content.ItemsSource = StoneCode.GetTypeContentListsByType(templist.SelectedValue.ToString());
                //templist_content.SelectedIndex = 0;
            }
        }
        public Codess _o;
        private void templist_content_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (templist_content.SelectedValue != null)
            {
                //int index = ((dynamic)templist_content.SelectedValue).Value;           
               
            }
        }
        private void CreateForm(Codess o) 
        {
            Form.Children.Clear();
            int i = 0;
            foreach (PropertyInfo item in o.GetType().GetProperties())
            {
                if (item != null &&!item.Name.Contains("代码"))
                {
                    string label_name = "";
                    var found = item.GetCustomAttribute<DescriptionAttribute>();
                    if (found != null) label_name = found.Description;
                     AddlabelTextAndTextName(i, label_name, item.Name);
                    i++;
                }
            }
        }
        private void AddlabelTextAndTextName(int index, string name_cn, string name_en, string name_content = "")
        {
            Form.RowDefinitions.Add(new RowDefinition());
            var textbox = GetTextBox(name_cn,name_en);
            Grid.SetRow(textbox, index);
            Form.Children.Add(textbox);

        }
        private TextBox GetTextBox(string title, string fiedname,string currentent = "")
        {
            TextBox textBox = new TextBox();
            textBox.Name = fiedname;
            textBox.Margin = new Thickness(0, 6, 0, 0);
            textBox.SetValue(TitleElement.TitleWidthProperty, new GridLength(70));
            textBox.SetValue(TitleElement.TitlePlacementProperty, TitlePlacementType.Left);
            textBox.SetValue(TitleElement.TitleProperty, title);
           // textBox.SetValue(StyleProperty, Resources["TextBoxExtend"]);
            textBox.Foreground = new SolidColorBrush(Colors.Gray);
            textBox.Text= currentent;
           // textBox.LostFocus += TextBox_LostFocus;
            textBox.TextChanged += ObjTextChanged;
            return textBox;
        }

        private async void ObjTextChanged(object sender, TextChangedEventArgs e)
        {
            isloding.Show();
            var txt = sender as TextBox;
            _o.GetType().GetProperty(txt.Name).SetValue(_o, txt.Text);
           
            isloding.Hide();
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            var check =(sender as MenuItem).Header.ToString();
            if (check == "全选") TextEditor.SelectAll();
            else if(check=="复制") Clipboard.SetText(TextEditor.SelectedText);
            else if (check == "复制所有内容") Clipboard.SetText(TextEditor.Text);

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            _o.Code = TextEditor.Text;
            int ddd=  new Litedb().InsertToDB(_o);
            templist.ItemsSource = new Litedb().Selects().Select(o=>o.Use);
           
        }

        private void edit_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click_2(object sender, RoutedEventArgs e)
        {
      
        }
    }
}
