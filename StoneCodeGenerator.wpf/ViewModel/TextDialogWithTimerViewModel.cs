using HandyControlDemo.Model;

namespace HandyControlDemo.ViewModel
{
    public class TextDialogWithTimerViewModel : NotificationBinding
    {
        private string showText;
        public string ShowText
        {
            get { return showText; }
            set
            {
                showText = value;
                OnPropertyChanged("ShowText");
            }
        }

        private int currentValue;
        public int CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

    }
}
