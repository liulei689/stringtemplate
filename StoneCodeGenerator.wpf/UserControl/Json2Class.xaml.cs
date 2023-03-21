using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// Json2Class.xaml 的交互逻辑
    /// </summary>
    public partial class Json2Class
    {
        public Json2Class()
        {
            InitializeComponent();
            TextEditor1.Text = "{\r\n  \"OptTypeList\": [\r\n    [\r\n      {\r\n        \"address\": \"宁波市鄞州区*****\",\r\n        \"divisionName\": \"********\",\r\n        \"tel\": \"010-12345678\",\r\n        \"divisionId\": \"813402\",\r\n        \"psummertime\": \"08:00\",\r\n        \"pwintertime\": \"18:00\",\r\n        \"streetId\": \"330212018\"\r\n      },\r\n      {\r\n        \"address\": \"宁波市鄞州区********\",\r\n        \"divisionName\": \"********\",\r\n        \"divisionId\": \"811904\",\r\n        \"streetId\": \"330212027\"\r\n      }\r\n    ]\r\n  ],\r\n  \"_TaskId\": \"53506a7943354748a1affdb9f635fb86\",\r\n  \"_Return\": \"000000\",\r\n  \"_ParentTaskId\": \"1f2bbefca759436a94017f55eed8512e\"\r\n}";
            TextEditor.Text = new StoneCodeGenerator.Lib.Json2Class.ClassGenerator().JsonToClasses(TextEditor1.Text);
        }

        private void TextEditor1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextEditor.Text = new StoneCodeGenerator.Lib.Json2Class.ClassGenerator().JsonToClasses(TextEditor1.Text);
            }
            catch { }

        }
    }
}
