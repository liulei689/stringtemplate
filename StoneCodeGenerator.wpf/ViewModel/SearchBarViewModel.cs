using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using System;

namespace HandyControlDemo.ViewModel
{
    public class SearchBarViewModel :  CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public RelayCommand<string> SearchCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(Search)).Value;

        private void Search(string key)
        {
            Growl.Info(key, "InfoMessage");
        }
    }
}
