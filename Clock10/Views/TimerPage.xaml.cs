using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Clock10.Views
{
    public sealed partial class TimerPage : Page, INotifyPropertyChanged
    {
        private static System.Timers.Timer aTimer;

        int hour = 0;
        int minute = 0;
        int second = 0;

        public TimerPage()
        {
            InitializeComponent();
            SetTimer();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void UpdateTime()
        {
            await Task.Delay(10);
                hour = (int)NumberBoxHour.Value;
                minute = (int)NumberBoxMinute.Value;
                second = (int)NumberBoxSecond.Value;
                timerTime.Text = $"{hour:00}:{minute:00}:{second:00}";
        }

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private async void timerToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (timerToggleButton.IsChecked == true)
            {
                aTimer.Start();
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                () =>
                {
                if (timerToggleButton.IsChecked == true)
                {
                    timerStateTextBox.Text = " Pause";
                    timerStateIcon.Symbol = Symbol.Pause; // Your UI update code goes here!
                }

            });

            }
            if (hour == 0 && minute == 0 && second == 0)
            {
                timerToggleButton_Unchecked(sender, e);
                timerToggleButton.IsChecked = false;
            }
        }

        private async void timerToggleButton_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            aTimer.Stop();

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
            () =>
            {
                if (timerToggleButton.IsChecked == true)
                {
                    timerStateTextBox.Text = " Start";
                    timerStateIcon.Symbol = Symbol.Play;// Your UI update code goes here!
                }

            });
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (second > 0)
                second -= 1;

            if (second == 0 && minute > 0)
            {
                second = 59;
                if (minute > 0)
                    minute -= 1;
            }
            else if (minute == 0 && hour > 0)
            {
                minute = 59;
                if (hour > 0)
                    hour -= 1;
            }
            
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
            () =>
            {
                if (timerToggleButton.IsChecked == true)
                {
                    timerTime.Text = $"{hour:00}:{minute:00}:{second:00}"; // Your UI update code goes here!
                }

            });

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
            () =>
            {
                if (second == 0 && minute == 0 && hour == 0 && timerToggleButton.IsChecked == true)
                {
                    timerStateTextBox.Text = " Start";
                    timerStateIcon.Symbol = Symbol.Play;
                    timerToggleButton.IsChecked = false;  
                    beep.MediaPlayer.Play();
                }
            });
        }

        private void NumberBoxHour_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args) => UpdateTime();
        private void NumberBoxMinute_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args) => UpdateTime();
        private void NumberBoxSecond_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args) => UpdateTime();

        private void timerResetButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            hour = 0;
            minute = 0;
            second = 0;
            aTimer.Dispose();
            timerTime.Text = "00:00:00";
        }
    }
}
