using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using NLua;
using StoneCodeGenerator.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            templist.SelectedIndex = 0;
            templist_content.ItemsSource = StoneCode.GetTypeContentListsByType(templist.Text);
            templist_content.SelectedIndex = 0;
            TextEditor.WordWrap = true;

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
                if (item != null && !item.Name.Contains("Path"))
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
            var textbox = GetTextBox(name_cn, name_en);
            Grid.SetRow(textbox, index);
            Form.Children.Add(textbox);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as System.Windows.Controls.Button);
            var dd = App.GetServiceByString(data.Tag.ToString());
            Type test = dd.GetType();
            MethodInfo mi1 = test.GetMethod(data.Name);
            string text = TextEditor.Text;
            object[] parameters = new object[1] { text };
            string dds = (string)mi1.Invoke(dd, parameters);
            TextEditor.Text = dds;
        }

        private TextBox GetTextBox(string title, string fiedname, string currentent = "")
        {
            TextBox textBox = new TextBox();
            textBox.Name = fiedname;
            textBox.Margin = new Thickness(0, 6, 0, 0);
            textBox.SetValue(TitleElement.TitleWidthProperty, new GridLength(70));
            textBox.SetValue(TitleElement.TitlePlacementProperty, TitlePlacementType.Left);
            textBox.SetValue(TitleElement.TitleProperty, title);
            // textBox.SetValue(StyleProperty, Resources["TextBoxExtend"]);
            textBox.Foreground = new SolidColorBrush(Colors.Gray);
            textBox.Text = currentent;
            // textBox.LostFocus += TextBox_LostFocus;
            textBox.TextChanged += ObjTextChanged;
            return textBox;
        }

        private async void ObjTextChanged(object sender, TextChangedEventArgs e)
        {
            isloding.Show();
            var txt = sender as TextBox;
            _o.GetType().GetProperty(txt.Name).SetValue(_o, txt.Text);
            TextEditor.Text = await StoneCode.GMethod(_o);
            isloding.Hide();
        }
        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            var check = (sender as MenuItem).Header.ToString();
            if (check == "全选") TextEditor.SelectAll();
            else if (check == "复制") Clipboard.SetText(TextEditor.SelectedText);
            else if (check == "复制所有内容") Clipboard.SetText(TextEditor.Text);
        }
        private void upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dd = App.GetServiceByString("IExcelToCSharpClassService");

                Type test = dd.GetType();
                MethodInfo mi1 = test.GetMethod("GetClassByExcelRowOne");
                string text = TextEditor.Text;
                object[] parameters = new object[1] { text };
                string dds = (string)mi1.Invoke(dd, parameters);

                TextEditor.Text = dds;
            }
            catch { }
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            // upload_Click(null,null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            List<Module> infoList = MiniExcel.Query<Module>("C:\\Users\\liu\\Documents\\工作簿1.xlsx", configuration: new OpenXmlConfiguration() { FillMergedCells = true }).ToList();
            var data = MergeModules(infoList);
            string generatedCode = CodeGenerator.GenerateListCode<Module>(data);
            TextEditor.Text = generatedCode;
            //git reset –hard 58fa47e9 回退到指定版本丢掉所有更改
        }
        public static List<Module> MergeModules(List<Module> modules)
        {
            // 使用Lookup来按SerialNumber分组
            var lookup = modules.ToLookup(m => m.序号);

            List<Module> mergedList = new List<Module>();

            foreach (var group in lookup)
            {
                // 对于每个组，取第一个Module作为基准，并拼接字符串类型的属性
                Module mergedModule = group.First();

                foreach (var module in group.Skip(1))
                {
                    // 这里假设除了Remarks外，其他字符串属性不需要合并（如果需要，可以类似处理）
                    mergedModule.数据 += "," + module.数据;
                }

                mergedList.Add(mergedModule);
            }

            return mergedList;
        }
        public class Module
        {
            public string 序号 { get; set; }
            public string 模块 { get; set; }
            public string 功能 { get; set; }
            public string 方向 { get; set; }
            public string 报头 { get; set; }
            public string 设备 { get; set; }
            public string 功能字节1 { get; set; }
            public string 功能字节2 { get; set; }
            public string 数据长度 { get; set; }
            public string 数据 { get; set; } // 假设这里是一个经过序列化的字符串表示，或者是一个base64编码的字符串
            public string 校验 { get; set; } // 假设这里也是一个经过处理的字符串表示
            public string 报尾 { get; set; }
            public string 备注 { get; set; }
        }

        public class CodeGenerator
        {
            // 泛型方法，T代表任意类型
            public static string GenerateListCode<T>(List<T> itemsToAdd)
            {
                StringBuilder sb = new StringBuilder();
                // 使用typeof(T).Name来获取类型名称，这里T在编译时是未知的，但在运行时是确定的
                string typeName = typeof(T).Name;
                sb.AppendLine($"List<{typeName}> list = new List<{typeName}>();");

                foreach (var item in itemsToAdd)
                {
                    // 使用反射获取T类型的所有属性
                    var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var propertyValues = properties.Select(prop =>
                    {
                        var value = prop.GetValue(item);
                        // 处理属性值，这里为了简单起见，我们直接调用ToString()
                        // 在实际应用中，你可能需要根据属性类型进行更复杂的处理
                        string valueString = value?.ToString() ?? "null";
                        // 对于字符串类型的属性，需要添加引号，并且处理引号转义（这里简化处理，不处理嵌套引号）
                        if (value is string)
                        {
                            valueString = $"\"{valueString.Replace("\"", "\\\"")}\"";
                        }
                        return $"{prop.Name} = {valueString}";
                    });

                    // 构造添加对象的代码字符串
                    string addLine = $"list.Add(new {typeName} {{ {string.Join(", ", propertyValues)} }});";
                    sb.AppendLine(addLine);
                }

                return sb.ToString();
            }
        }
        Lua lua = new Lua();

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (Lua lua = new Lua())
            {
                // 加载并执行包含中文字符串的Lua脚本
                string luaScript = @"
                function greet(name)
                    return '你好, ' .. name
                end
            ";
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile("2.txt");
                //lua.DoString(luaScript);

                // 调用Lua函数并传递中文字符串参数
                LuaFunction greetFunc = lua["greet"] as LuaFunction;
                if (greetFunc != null)
                {
                    object[] results = greetFunc.Call("张三");
                    string greeting = (string)results[0];
                    TextEditor.Text = greeting;
                }
            }


        }
    }
}
