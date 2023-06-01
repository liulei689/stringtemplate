using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using StoneCodeGenerator.Lib;
using StoneCodeGenerator.Lib.Model;
using StoneCodeGenerator.Service.Interface;
using StoneCodeGenerator.Service.Services;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using static ICSharpCode.AvalonEdit.Document.TextDocumentWeakEventManager;
using static System.Net.Mime.MediaTypeNames;
using TextBox = HandyControl.Controls.TextBox;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// PropertyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class AFunCode
    {

        public AFunCode()
        {
            InitializeComponent();
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);
            TextEditor.Text = "using HandyControlDemo.Model;\nusing ICSharpCode.AvalonEdit;\r\nusing System.Windows;\n\nnamespace HandyControlDemo.UserControl\n{\n    /// <summary>\n    /// PropertyGrid.xaml 的交互逻辑\n    /// </summary>\n    public partial class AFunCode\n    {\n        public AFunCode()\n        {\n            InitializeComponent();\n            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);\n            TextEditor.Text = \"\";\n            DemoModel = new PropertyGridDemoModel\n            {\n                String = \"TestString\",\n                Enum = Gender.Female,\n                Boolean = true,\n                Integer = 98,\n                VerticalAlignment = VerticalAlignment.Stretch\n            };\n\n            //DataContext = this;\n        }\n\n        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(\n            \"DemoModel\", typeof(PropertyGridDemoModel), typeof(PropertyGrid), new PropertyMetadata(default(PropertyGridDemoModel)));\n\n        public PropertyGridDemoModel DemoModel\n        {\n            get => (PropertyGridDemoModel)GetValue(DemoModelProperty);\n            set => SetValue(DemoModelProperty, value);\n        }\n    }\n}\n";
            templist.ItemsSource = StoneCode.GetTypeLists();
            templist.SelectedIndex=0;
            templist_content.ItemsSource= StoneCode.GetTypeContentListsByType(templist.Text);
            templist_content.SelectedIndex=0;
            TextEditor.WordWrap = true;
            PlusToUi();
        }

    

        private void templist_Selected(object sender, RoutedEventArgs e)
        {
            templist_content.ItemsSource = StoneCode.GetTypeContentListsByType(templist.SelectedValue.ToString());
            templist_content.SelectedIndex = 0;

        }
        public object _o;
        private void templist_content_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (templist_content.SelectedValue != null)
            {
                int index = ((dynamic)templist_content.SelectedValue).Value;
                _o = StoneCode.list[index - 1];
                CreateForm(_o);
            }
        }
        private void CreateForm(object o) 
        {
            Form.Children.Clear();
            int i = 0;
            foreach (PropertyInfo item in o.GetType().GetProperties())
            {
                if (item != null &&!item.Name.Contains("Path"))
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
        private void PlusToUi()
        {
            App.interfaceTypes.ForEach(type => {
                contents.Children.Add(GetButton(type.Name, type.Name));
                
            });
        }
        private System.Windows.Controls.Button GetButton(string title, string fiedname, string currentent = "")
        {
           System.Windows.Controls.Button button = new System.Windows.Controls.Button();
           
            button.Name = fiedname;
            button.Margin = new Thickness(0, 6, 0, 0);
            button.SetValue(StyleProperty, Resources["ButtonDashedSuccess"]);
            button.Content = title;
            button.Click += Button_Click; ;
            return button;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var data= (sender as System.Windows.Controls.Button);
       var dd=  App.GetServiceByString(data.Name);
                        string text = TextEditor.Text;
                TextEditor.Text= ((dynamic)dd).InputToOut(text);
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
            var txt=sender as TextBox;           
            _o.GetType().GetProperty(txt.Name).SetValue(_o, txt.Text);
            TextEditor.Text = await StoneCode.GMethod(_o);
            isloding.Hide();
        }



        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            var check =(sender as MenuItem).Header.ToString();
            if (check == "全选") TextEditor.SelectAll();
            else if(check=="复制") Clipboard.SetText(TextEditor.SelectedText);
            else if (check == "复制所有内容") Clipboard.SetText(TextEditor.Text);

        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             var dd=  App.GetServiceByString("IExcelToCSharpClassService");
        
             // var ds=  App.GetService <IExcelToCSharpClassService>();
                string text = TextEditor.Text;
                TextEditor.Text= ((dynamic)dd).InputToOut(text);
            }
            catch { }

        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
           // upload_Click(null,null);
        }
    }
}
