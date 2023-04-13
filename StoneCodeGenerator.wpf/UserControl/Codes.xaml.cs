using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using HandyControlDemo.Helper;
using ICSharpCode.AvalonEdit;
using KJAutoCompleteTextBox;
using LiteDB;
using MongoDB.Driver;
using StoneCodeGenerator.Lib;
using StoneCodeGenerator.Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ComboBox = HandyControl.Controls.ComboBox;
using TextBox = HandyControl.Controls.TextBox;

namespace HandyControlDemo.UserControl
{
    /// <summary>
    /// PropertyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class Codes
    {
        public Codess _o;
        public List<Codess> _lo;
        public Codes()
        {
            InitializeComponent();
      
            textBoxComplete.SelectComBox += TextBoxComplete_SelectComBox;
            textBoxComplete.TextChange += TextBoxComplete_TextChange;
            
        }
        private void SelectData(Codess ddsdsadd) 
        {
            if (ddsdsadd != null)
            {
                _o = ddsdsadd;
                CreateForm(_o);
                TextEditor.Text = ddsdsadd.Code;
                Task.Run(() => {
                    ddsdsadd.ReadCount++;
                    ddsdsadd.ReadTime = DateTime.Now.ToString();
                    ddsdsadd.Use = ddsdsadd.Use.Replace(ddsdsadd._id + ".", "");
                    new Litedb().UpdateOneToDB(ddsdsadd);
                    UpdateMongodb(ddsdsadd);
                });
            }
            else TextEditor.Text = "";
            all_read_count.Status = _lo.Sum(o => o.ReadCount)+1;
        }
        private void TextBoxComplete_TextChange()
        {
            var ddsdsadd = _lo.Find(o => o.Use.ToLower().Contains(textBoxComplete.Text.ToLower()));
            SelectData(ddsdsadd);
        }

        private void TextBoxComplete_SelectComBox()
        {
            var ddsdsadd = _lo.Find(o => o.Use.Contains(textBoxComplete.Text.Replace(o._id+".","")));
            SelectData(ddsdsadd);
        }

        private void templist_Selected(object sender, RoutedEventArgs e)
        {
            if (templist.SelectedValue != null)
            {

                var ddd = templist.SelectedValue.ToString();
                var ddsdsadd = _lo.Find(o => ddd.Contains(o.Use));
                SelectData(ddsdsadd);
            }
        }
        private void CreateForm(Codess o)
        {
            try
            {
                Form.Children.Clear();
                int i = 0;
                foreach (PropertyInfo item in o.GetType().GetProperties())
                {
                    if (item != null && !item.Name.Contains("Code") )
                    {
                        string label_name = "";
                        var found = item.GetCustomAttribute<DescriptionAttribute>();
                        if (found != null) label_name = found.Description;
                        var iscombox = item.GetCustomAttribute<IsCombox>();
                        var isreadonly = item.GetCustomAttribute<ReadOnlyAttribute>();
                        if (iscombox != null && iscombox.Canuse)
                        {
                            AddlabelTextAndTextName(i, label_name, item.Name, type: "combox", item.GetValue(o, null).ToString());
                        }
                        else if (item.PropertyType.IsEnum)
                        {
                            var type = Enum.GetNames(item.PropertyType);
                            AddlabelTextAndTextName(i, label_name, item.Name, type: "enum", item.GetValue(o, null).ToString(), type);
                        }
                        else
                            AddlabelTextAndTextName(i, label_name, item.Name, type: "textbox", item.GetValue(o, null).ToString(), readonlys: isreadonly != null);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                HandyControl.Controls.Growl.Error(ex.Message);
            }
        }
        private void AddlabelTextAndTextName(int index, string name_cn, string name_en, string type, string name_content = "", string[] ls = null, bool readonlys = false)
        {
            Form.RowDefinitions.Add(new RowDefinition());
            if (type == "combox")
            {
                if (name_en == "From")
                    ls = _lo.GroupBy(o => o.From).Select(P => P.OrderByDescending(x => x.Use).First()).Select(o => o.From).ToArray();
                else if (name_en == "Technical")
                    ls = _lo.GroupBy(o => o.Technical).Select(P => P.OrderByDescending(x => x.Technical).First()).Select(o => o.Technical).ToArray();
                else if (name_en == "Language")
                    ls = _lo.GroupBy(o => o.Language).Select(P => P.OrderByDescending(x => x.Language).First()).Select(o => o.Language).ToArray();

                var combox = GetComboBox(name_cn, name_en, ls, true, name_content);
                Grid.SetRow(combox, index);
                Form.Children.Add(combox);
            }
            else if (type == "enum")
            {
                var combox = GetComboBox(name_cn, name_en, ls, false, name_content);
                Grid.SetRow(combox, index);
                Form.Children.Add(combox);
            }
            else if (type == "textbox")
            {
                var textbox = GetTextBox(name_cn, name_en, name_content, readonlys);
                Grid.SetRow(textbox, index);
                Form.Children.Add(textbox);
            }
        }
        private TextBox GetTextBox(string title, string fiedname, string currentent = "", bool readonlys = false)
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
            textBox.IsReadOnly = readonlys;
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
            comboBox.IsEditable = IsEditable;

            comboBox.ItemsSource = conent;
            if (IsEditable)
                comboBox.Text = currentent;
            else
                comboBox.SelectedValue = currentent;
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

        private void ObjTextChanged(object sender, TextChangedEventArgs e)
        {
            //isloding.Show();
            var txt = sender as TextBox;
            if (txt.Name == "Use")
            {
                if (_lo.FindAll(o => o.Use == txt.Text).Count() == 0)
                {
                    foreach (var control in Form.Children)
                    {
                        if (control is TextBox && (control as TextBox).Name == "UseDetail")
                        {
                            (control as TextBox).Text = txt.Text;
                        }
                        if (control is TextBox && (control as TextBox).Name == "TimeUpate")
                        {
                            (control as TextBox).Text = DateTime.Now.ToString();
                        }
                        if (control is TextBox && (control as TextBox).Name == "CreateTime")
                        {
                            (control as TextBox).Text = DateTime.Now.ToString();
                        }
                    }
                    try
                    {
                        TextEditor.Text = Clipboard.GetText();
                    }
                    catch { TextEditor.Text = ""; }
                }

            }
            //_o.GetType().GetProperty(txt.Name).SetValue(_o, txt.Text);

            //isloding.Hide();
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var check = (sender as MenuItem).Header.ToString();
                if (check == "全选") TextEditor.SelectAll();
                else if (check == "复制") Clipboard.SetText(TextEditor.SelectedText);
                else if (check == "复制所有内容") Clipboard.SetText(TextEditor.Text);
            }
            catch { }

        }
        
            private async void update_Click(object sender, RoutedEventArgs e)
        {
            isloding.Show();

            if (_o.Use != "")
            {
                foreach (var control in Form.Children)
                {
                    if (control is ComboBox)
                    {
                        var type = _o.GetType().GetProperty((control as ComboBox).Name);
                        if (type.PropertyType.IsEnum)
                        {
                            type.SetValue(_o, Enum.Parse(type.PropertyType, (control as ComboBox).Text));
                        }
                        else if (type.PropertyType == typeof(string))
                            type.SetValue(_o, (control as ComboBox).Text);
                    }
                    if (control is TextBox)
                    {
                        var type = _o.GetType().GetProperty((control as TextBox).Name);
                        if (type.PropertyType == typeof(int))
                            _o.GetType().GetProperty((control as TextBox).Name).SetValue(_o, int.Parse((control as TextBox).Text));
                        else
                            _o.GetType().GetProperty((control as TextBox).Name).SetValue(_o, (control as TextBox).Text);
                    }
                }
          
                _o.Code = TextEditor.Text;
                _o.Use = _o.Use.Replace(_o._id+".", "");
                _o.TimeUpate = DateTime.Now.ToString();
                var ov = await Task.Run(() =>
                {
                    var data = new Litedb().UpdateOneToDB(_o);
          
                    return data;

                });
                _lo = await Task.Run(() =>
                {
                    return new Litedb().Selects();
                });
                RefreshTheList(_lo);
                if (!ov)
                {
                    HandyControl.Controls.Growl.Error("操作失败！");
                    return;
                }
                await Task.Run(() =>
                {
                    UpdateMongodb(_o);
                });
              
                    HandyControl.Controls.Growl.Warning("修改成功");



            }
            else HandyControl.Controls.Growl.Error("用处不可为空");
            isloding.Hide();
        }
            private async void add_Click(object sender, RoutedEventArgs e)
        {
            isloding.Show();

            if (_o.Use != "")
            {
                foreach (var control in Form.Children)
                {
                    if (control is ComboBox)
                    {
                        var type = _o.GetType().GetProperty((control as ComboBox).Name);
                        if (type.PropertyType.IsEnum)
                        {
                            type.SetValue(_o, Enum.Parse(type.PropertyType, (control as ComboBox).Text));
                        }
                        else if (type.PropertyType == typeof(string))
                            type.SetValue(_o, (control as ComboBox).Text);
                    }
                    if (control is TextBox)
                    {
                       var type= _o.GetType().GetProperty((control as TextBox).Name);
                        if (type.PropertyType == typeof(int))
                            _o.GetType().GetProperty((control as TextBox).Name).SetValue(_o, int.Parse((control as TextBox).Text));
                        else
                            _o.GetType().GetProperty((control as TextBox).Name).SetValue(_o, (control as TextBox).Text);
                    }
                }
                var isexist = new Litedb().UseIsExist(_o);
                if (isexist)
                {
                    HandyControl.Controls.Growl.Warning("已存在" + _o.Use);
                    isloding.Hide();
                    return;
                }
                _o.TimeUpate = DateTime.Now.ToString();
                _o.CreateTime = _o.TimeUpate;
                _o.ReadTime = _o.TimeUpate;
                _o.Code = TextEditor.Text;
               _o.ReadCount = 0;
                int staus_id = -1;
                var ov = await Task.Run(() =>
                {
                    var data = new Litedb().InsertToDB(_o, out int status);
                    staus_id = status;
                    return data;

                });
                _lo = await Task.Run(() =>
                {
                    return new Litedb().Selects();
                });
                RefreshTheList(_lo);
                if (ov == null)
                {
                    HandyControl.Controls.Growl.Error("操作失败！");
                }
                await Task.Run(() =>
                {
                    UpdateMongodb(ov);
                });
                if (staus_id == 1)
                {
                    HandyControl.Controls.Growl.Warning("修改成功");

                }
                else if (staus_id == 2) HandyControl.Controls.Growl.Success("添加成功");


            }
            else HandyControl.Controls.Growl.Error("用处不可为空");
            isloding.Hide();
        }
        #region 刷新列表
        /// <summary>
        /// 刷新列表
        /// </summary>
        public void RefreshTheList(List<Codess> cs)
        {
            _lo = cs;
            var list = _lo.Select(p =>
            {
                p.Use = p._id +"."+ p.Use;
                return p;
            }).Select(o=>o.Use). ToList();
            DateTime timetoday = DateTime.Today;
            DateTime timelastweek = DateTime.Today.AddDays(-7);
            DateTime timelastmons = DateTime.Today.AddMonths(-1);
            var ltoday = _lo.FindAll(o => o.CreateTime.CompareTo(timetoday.ToString()) < 0);
            var llastweek = _lo.FindAll(o => o.CreateTime.CompareTo(timelastweek.ToString()) > 0);
            var llastmons = _lo.FindAll(o=>o.CreateTime.CompareTo(timelastmons.ToString()) > 0);
            today_add_count.Status = ltoday.Count();
            week_add_count.Status= llastweek.Count();
            mon_add_count.Status=llastmons.Count();
            all_read_count.Status= _lo.Sum(o => o.ReadCount);
            all_count.Status= _lo.Count();
            var today_update = _lo.FindAll(o => o.TimeUpate.CompareTo(timetoday.ToString()) < 0);
            today_eddit_count.Status = today_update.Count();
            for (int i = 0; i < list.Count(); i++)
                textBoxComplete.AddItem(new AutoCompleteEntry(list[i], null));
            templist.ItemsSource = list;
           // tixing.Content = _lo.Count + "条";
            if (_o == null) templist.SelectedIndex = 0;
            else
                templist.SelectedValue = _o._id;
            textBoxComplete.ClearItem();
            for (int i = 0; i < list.Count(); i++)
                textBoxComplete.AddItem(new AutoCompleteEntry(list[i], null));
        }
        #endregion

        #region 更新mongodb
        /// <summary>
        /// 更新mongodb
        /// </summary>
        public void UpdateMongodb(Codess cs)
        {
            var mongodb = MongoDbClient.GetInstance("mongodb://124.221.160.244:83/", "同步库");
            // 创建筛选器定义
            // FilterDefinition<Codess> filter = Builders<Codess>.Filter.Eq("name", "John");
            // 创建更新器定义 新增
            UpdateDefinition<Codess> update = Builders<Codess>.Update
                .Set(o => o.Use, cs.Use)
                .Set(o => o.Technical, cs.Technical)
                .Set(o => o.UseDetail, cs.UseDetail)
                .Set(o => o.From, cs.From)
                .Set(o => o.Language, cs.Language)
                .Set(o => o.TimeUpate, cs.TimeUpate)
                .Set(o => o.CreateTime, cs.CreateTime)
                .Set(o => o.Code, cs.Code)
                .Set(o => o.ReadCount, cs.ReadCount)
                 .Set(o => o.ReadTime, cs.ReadTime)
                .Set(o => o._id, cs._id);
            // 更新集合中的文档
            mongodb.UpdateOne<Codess>("代码库", o => o._id == cs._id, update, true);

        }
        #endregion
        #region 查询mongodb
        /// <summary>
        /// 查询mongodb
        /// </summary>
        public List<Codess> SelectMongodb()
        {
            var mongodb = MongoDbClient.GetInstance("mongodb://124.221.160.244:83/", "同步库");
            // 创建筛选器定义
            //  FilterDefinition<Codess> filter = Builders<Codess>.Filter.Eq("name", "John");//等于
            FilterDefinition<Codess> filter = Builders<Codess>.Filter.Ne("_id", "");//不等于
            return mongodb.GetCollection<Codess>("代码库").Find(filter).ToList();

        }
        #endregion
        #region 删除mongodb
        /// <summary>
        /// 查询mongodb
        /// </summary>
        public long DeleteMongoById(string id)
        {
            var mongodb = MongoDbClient.GetInstance("mongodb://124.221.160.244:83/", "同步库");
            FilterDefinition<Codess> filter = Builders<Codess>.Filter.Eq("_id", id);//等于
            return mongodb.Delete("代码库", filter);
        }
        #endregion
        //同步芒果到本地到 下载
        private async void MongoToLite(object sender, RoutedEventArgs e)
        {
            await MongoToLiteAnysc();
        }
        //同步芒果到本地到 下载
        private async Task MongoToLiteAnysc()
        {
            isloding.Show();
            upload.IsEnabled = false;
            down.IsEnabled = false;
            var data = SelectMongodb();
            await Task.Run(() =>
            {
                try
                {
                    if (data.Count() == 0) return;
                    var db = new Litedb();
                    db._db.GetCollection<Codess>("代码库").DeleteAll();
                    for (int i = 0; i < data.Count(); i++)
                    {
                        db.InsertMongoToDB(data[i]);
                    }
                }
                catch (Exception ex)
                {
                    // HandyControl.Controls.Growl.Error(ex.Message);
                }
            });
            RefreshTheList(data);
            upload.IsEnabled = true;
            down.IsEnabled = true;
            isloding.Hide();
        }
        //同步本地到芒果 上传
        private async void LiteToMongo(object sender, RoutedEventArgs e)
        {
       
            isloding.Show();
            upload.IsEnabled = false;
            down.IsEnabled = false;
            var data = new Litedb().Selects();
            await Task.Run(() =>
            {
                try
                {                
                    var mongodb = MongoDbClient.GetInstance("mongodb://124.221.160.244:83/", "同步库");
                    var cc = mongodb.GetCollection<Codess>("代码库");
                    cc.Database.DropCollection("代码库");
                    mongodb.InsertMany("代码库", data);                
                }
                catch (Exception ex)
                {
                    //  HandyControl.Controls.Growl.Error(ex.Message);
                }
            });
            RefreshTheList(data);
            upload.IsEnabled=true;
            down.IsEnabled = true;
            isloding.Hide();
        }
        private void delete_Click_2(object sender, RoutedEventArgs e)
        {
            new Litedb().DeleteOne(_o);
            var dd = DeleteMongoById(_o._id);
            var data = new Litedb().Selects();
            RefreshTheList(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await MongoToLiteAnysc();
            _lo = new Litedb().Selects();
            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(TextEditor);
            RefreshTheList(_lo);
            TextEditor.WordWrap = true;
            if (_lo.Count() == 0)
            {
                CreateForm(new Codess());
                _o = new Codess();
            }
        }
    }
}
