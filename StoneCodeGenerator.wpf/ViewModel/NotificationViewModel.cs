
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Model;
using HandyControlDemo.UserControl.Window;
using System;

namespace HandyControlDemo.ViewModel
{
    public class NotificationViewModel : NotificationBinding
    {
        private string textInfo;
        public string TextInfo
        {
            get { return textInfo; }
            set
            {
                textInfo = value;
                OnPropertyChanged("TextInfo");
            }
        }

        private Array animationTypes;
        public Array AnimationTypes
        {
            get { return animationTypes; }
            set
            {
                animationTypes = value;
                OnPropertyChanged("AnimationTypes");
            }
        }

        private ShowAnimation animationType;
        public ShowAnimation AnimationType
        {
            get { return animationType; }
            set
            {
                animationType = value;
                OnPropertyChanged("AnimationType");
            }
        }

        private bool staysOpen;
        public bool StaysOpen
        {
            get { return staysOpen; }
            set
            {
                staysOpen = value;
                OnPropertyChanged("StaysOpen");
            }
        }

        public RelayCommand OpenCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Notification.Show(new AppNotification(TextInfo), AnimationType, StaysOpen))).Value;

        public NotificationViewModel()
        {
            TextInfo = "HandyControl";
            AnimationTypes = Enum.GetValues(typeof(ShowAnimation));
        }
    }
}
