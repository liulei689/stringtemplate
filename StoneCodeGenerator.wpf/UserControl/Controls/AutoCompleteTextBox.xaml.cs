﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using System.Collections.ObjectModel;
using System.Timers;
using System.Security.Cryptography;
using System;

namespace KJAutoCompleteTextBox
{
    /// <summary>
    /// AutoCompleteTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class AutoCompleteTextBox : Canvas
    {
        #region 成员变量

        private VisualCollection controls;
        private TextBox textBox;
        public ComboBox comboBox;
        private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;
        /// <summary>
        /// 键盘上下键
        /// </summary>
        public event Action KeyDownUp;
        /// <summary>
        /// 选择了下拉框
        /// </summary>
        public event Action SelectComBox;
        /// <summary>
        /// 文本变化
        /// </summary>
        public event Action TextChange;

        #endregion 成员变量

        #region 构造函数

        public AutoCompleteTextBox()
        {
            controls = new VisualCollection(this);
            InitializeComponent();

            autoCompletionList = new ObservableCollection<AutoCompleteEntry>();
            searchThreshold = 0;        // default threshold to 2 char
            delayTime = 100;

            // set up the key press timer
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            // set up the text box and the combo box
            comboBox = new ComboBox();
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
           // comboBox.Foreground = new Brush(Color.FromRgb(1,1,1));
            Panel.SetZIndex(comboBox, -1);
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);

            textBox = new TextBox();
            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.GotFocus += new RoutedEventHandler(textBox_GotFocus);
            textBox.KeyUp += new KeyEventHandler(textBox_KeyUp);
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            textBox.MouseDoubleClick += TextBox_MouseDoubleClick;
            controls.Add(comboBox);
            controls.Add(textBox);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textBox.Text = "";
        }

        #endregion 构造函数

        #region 成员方法

        public string Text
        {
            get { return textBox.Text; }
            set
            {
                insertText = true;
                textBox.Text = value;
            }
        }

        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }

        /// <summary>
        /// 添加Item
        /// </summary>
        /// <param name="entry"></param>
        public void AddItem(AutoCompleteEntry entry)
        {
            autoCompletionList.Add(entry);
        }


        /// <summary>
        /// 添加源-人工增加部分
        /// </summary>
        public void AddItemSource(List<AutoCompleteEntry> tlist)
        {
            for (int i = 0; i < tlist.Count; i++)
            {
                AddItem(tlist[i]);
            }

        }


        /// <summary>
        /// 清空Item
        /// </summary>
        /// <param name="entry"></param>
        public void ClearItem()
        {
            autoCompletionList.Clear();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
                textBox.Text = cbItem.Content.ToString();
                SelectComBox?.Invoke();
            }
        }

        private void TextChanged()
        {
            try
            {
                comboBox.Items.Clear();
                if (textBox.Text.Length >= searchThreshold)
                {
                    foreach (AutoCompleteEntry entry in autoCompletionList)
                    {
                        foreach (string word in entry.KeywordStrings)
                        {
                            if (word.Contains(textBox.Text))
                            {
                                ComboBoxItem cbItem = new ComboBoxItem();
                                cbItem.Content = entry.ToString();
                                comboBox.Items.Add(cbItem);
                                break;
                            }
                            //if (word.StartsWith(textBox.Text, StringComparison.CurrentCultureIgnoreCase))
                            //{
                            //    ComboBoxItem cbItem = new ComboBoxItem();
                            //    cbItem.Content = entry.ToString();
                            //    comboBox.Items.Add(cbItem);
                            //    break;
                            //}
                        }
                    }
                    comboBox.IsDropDownOpen = comboBox.HasItems;
                }
                else
                {
                    comboBox.IsDropDownOpen = false;
                }
                TextChange?.Invoke();
            }
            catch { }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;

            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
            
        }

        //获得焦点时
        public void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) 
                insertText = false;

            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        public void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox.IsInputMethodEnabled == true)
            {
                comboBox.IsDropDownOpen = false;
            }
        }
        /// <summary>
        /// 按向下按键时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && comboBox.IsDropDownOpen == true)
            {
               
                comboBox.Focus();
                //if(comboBox.SelectedIndex<comboBox.Items.Count-1)

                KeyDownUp?.Invoke();
                // comboBox.SelectedIndex= ddsad;
                //else comboBox.SelectedIndex = 0;


            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            textBox.Arrange(new Rect(arrangeSize));
            comboBox.Arrange(new Rect(arrangeSize));
            return base.ArrangeOverride(arrangeSize);
        }

        protected override Visual GetVisualChild(int index)
        {
            return controls[index];
        }

        protected override int VisualChildrenCount
        {
            get { return controls.Count; }
        }

        #endregion 成员方法
    }
}
