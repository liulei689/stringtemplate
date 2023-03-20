using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using StoneCodeGenerator.Lib;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ICSharpCode.AvalonEdit.Document.TextDocumentWeakEventManager;
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
        }

        private async void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isloding.Show();
            //var funmodel = new FunCode_Defult_1() { FUseFor = FUseFor.Text,FCment=FCment.Text,FSout=FSout.Text,FTmpPath=templist.Text };
            //TextEditor.Text = await StoneCode.GMethod(funmodel);
            isloding.Hide();
        }

        private void templist_Selected(object sender, RoutedEventArgs e)
        {
            templist_content.ItemsSource = StoneCode.GetTypeContentListsByType(templist.SelectedValue.ToString());
            templist_content.SelectedIndex = 0;

        }
        class ds { public string Key { get; set; } public int Value { get; set; } }

        private void templist_content_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (templist_content.SelectedValue != null)
            {
                int index = ((dynamic)templist_content.SelectedValue).Value;
                var obj = StoneCode.list[index - 1];
                CreateForm(obj);
                var DD1 = templist.Text;
            }
        }
        private void CreateForm(object o) 
        {  
            int i = 0;
            foreach (PropertyInfo item in o.GetType().GetProperties())
            {
                if (item != null)
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
            var textbox = GetTextBox(name_cn);
            Grid.SetRow(textbox, index);
            Form.Children.Add(textbox);

        }
        private TextBox GetTextBox(string title, string currentent = "")
        {
            TextBox textBox = new TextBox();
            textBox.Name = title;
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

        private void ObjTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
