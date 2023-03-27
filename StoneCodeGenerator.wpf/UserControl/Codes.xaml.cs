﻿using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using ICSharpCode.AvalonEdit;
using KJAutoCompleteTextBox;
using StoneCodeGenerator.Lib;
using StoneCodeGenerator.Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ICSharpCode.AvalonEdit.Document.TextDocumentWeakEventManager;
using static System.Net.Mime.MediaTypeNames;
using ComboBox = HandyControl.Controls.ComboBox;
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
           var dd= new Litedb().Selects();
            tixing.Content = dd.Count + "条";
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);
            var list= dd.Select(o => o.Use);
            templist.ItemsSource = list;
            templist.SelectedIndex = 0;     
            TextEditor.WordWrap = true;
            if (list.Count() == 0)
            {
                CreateForm(new Codess());
                _o = new Codess();
            }
            textBoxComplete.AddItem(new AutoCompleteEntry("上海", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("北京", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("济南", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("青岛", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("天津", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("黑龙江", null));
            textBoxComplete.AddItem(new AutoCompleteEntry("聊城", null));

            //方法2，使用list<AutoCompleteEntry>,自写ADDItemSource方法，直接通过泛型赋值。
            List<AutoCompleteEntry> tlist = new List<AutoCompleteEntry>();
            tlist.Add(new AutoCompleteEntry("第九人民医院", null));
            tlist.Add(new AutoCompleteEntry("第八人民医院", null));
            tlist.Add(new AutoCompleteEntry("第七人民医院", null));
            tlist.Add(new AutoCompleteEntry("第五人民医院", null));
            textBoxComplete.AddItemSource(tlist);
        }
        private void templist_Selected(object sender, RoutedEventArgs e)
        {
            if (templist.SelectedValue != null)
            {
              
                var ddd = templist.SelectedValue.ToString();
                var ddsdsadd = new Litedb().Selects().Find(o => o.Use == ddd);
                if (ddsdsadd != null)
                {
                    _o = ddsdsadd;
                    CreateForm(_o);
                    TextEditor.Text = ddsdsadd.Code;
                }
            
            }
        }
        public Codess _o;
        private void CreateForm(Codess o) 
        {
            Form.Children.Clear();
            int i = 0;
            foreach (PropertyInfo item in o.GetType().GetProperties())
            {
                if (item != null &&!item.Name.Contains("Code") && !item.Name.Contains("_id"))
                {
                    string label_name = "";
                    var found = item.GetCustomAttribute<DescriptionAttribute>();
                    if (found != null) label_name = found.Description;
                    if (item.PropertyType.IsEnum)
                        {
                        var type = Enum.GetNames(item.PropertyType);
                        AddlabelTextAndTextName(i, label_name, item.Name, item.GetValue(o, null).ToString(),type);

                    }
                    else
                    AddlabelTextAndTextName(i, label_name, item.Name,item.GetValue(o,null).ToString());
                    i++;
                }
            }
        }
        private void AddlabelTextAndTextName(int index, string name_cn, string name_en, string name_content = "",string[] ls=null)
        {
            Form.RowDefinitions.Add(new RowDefinition());
            if (ls != null)
            {
                var combox = GetComboBox(name_cn, name_en, ls, true, name_content);
                Grid.SetRow(combox, index);
                Form.Children.Add(combox);
            }
            else
            {
                var textbox = GetTextBox(name_cn, name_en, name_content);
                Grid.SetRow(textbox, index);
                Form.Children.Add(textbox);
            }

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
            if(fiedname== "TimeUpate") textBox.IsReadOnly = true;
           // textBox.LostFocus += TextBox_LostFocus;
            textBox.TextChanged += ObjTextChanged;
            return textBox;
        }
        private ComboBox GetComboBox(string title, string name_en, string[] conent, bool IsEditable, string currentent = "")
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Name = name_en;
            comboBox.Margin = new Thickness(0, 6, 0, 0);
            comboBox.SetValue(TitleElement.TitleWidthProperty, new GridLength(70));
            comboBox.SetValue(TitleElement.TitlePlacementProperty, TitlePlacementType.Left);
            comboBox.SetValue(TitleElement.TitleProperty, title);
            // comboBox.SetValue(InfoElement.PlaceholderProperty, "L222e");
          

            comboBox.ItemsSource = (System.Collections.IEnumerable)conent;
            //if (IsEditable)
            //    comboBox.Text = currentent;
            //else
                comboBox.SelectedValue = currentent;
            //comboBox.PreviewMouseDown += PreviewMouseDownChanged;
            comboBox.SelectionChanged += Selection_Changed;
            comboBox.Foreground = new SolidColorBrush(Colors.Gray);
            // comboBox.SetValue(ComboBox.StyleProperty, Resources["ComboBoxExtend"]);
            return comboBox;
        }
        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
         
                var sc = sender as ComboBox;
                var type = _o.GetType().GetProperty(sc.Name);
                if (type.PropertyType.IsEnum)
                {
                    _o.GetType().GetProperty(sc.Name).SetValue(_o, Enum.Parse(type.PropertyType, sc.SelectedValue.ToString()));
                }
            
            }
  
        private  void ObjTextChanged(object sender, TextChangedEventArgs e)
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
            if (_o.Use != "")
            {
                _o.TimeUpate = DateTime.Now.ToString();
                _o._id = _o.Use;
                _o.Code = TextEditor.Text;
                int ddd = new Litedb().InsertToDB(_o);
                var data = new Litedb().Selects();
                templist.ItemsSource = data.Select(o => o.Use);
                tixing.Content = data.Count+"条";
                templist.SelectedValue = _o._id;

            }

        }

        private void delete_Click_2(object sender, RoutedEventArgs e)
        {
            new Litedb().DeleteOne(_o);
            var data = new Litedb().Selects();
            templist.ItemsSource = data.Select(o => o.Use);
            tixing.Content =  data.Count + "条";
            templist.SelectedIndex = 0;


        }

   

      
    }
}