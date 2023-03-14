using Flurl.Util;
using HandyControl.Tools.Extension;
using HandyControlDemo.Model;
using ICSharpCode.AvalonEdit;
using StoneCodeGenerator.Lib;
using StoneCodeGenerator.Lib.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;

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
            var funmodel = new FunCode_Defult_1() { FUseFor = FUseFor.Text,FCment=FCment.Text,FSout=FSout.Text,FTmpPath=templist.Text };
            TextEditor.Text = await StoneCode.GMethod(funmodel);
            isloding.Hide();
        }

        private void templist_Selected(object sender, RoutedEventArgs e)
        {
           var DD= templist_content.Text;
            var DD1 = templist.Text;

        }
        class ds { public string Key { get; set; } public int Value { get; set; } }

        private void templist_content_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var DD = templist_content.SelectedItem as ds ;
            
            var DD1 = templist.Text;
        }
    }
}
