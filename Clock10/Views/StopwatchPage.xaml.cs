using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using Windows.UI.Core;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Core;

namespace Clock10.Views
{
    public sealed partial class StopwatchPage : Page, INotifyPropertyChanged
    {
        private static System.Timers.Timer aTimer;

        public StopwatchPage()
        {
            InitializeComponent();
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

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(1);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                if (stopwatchToggleButton.IsChecked == true)
                {
                    TimeSpan ts = stopWatch.Elapsed;
                    stopwatchTime.Text = $"{ts}"; // Your UI update code goes here!
                }

            });
        }

        Stopwatch stopWatch = new Stopwatch();
        private void stopwatchToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SetTimer();
            
            if (stopwatchToggleButton.IsChecked == true)
            {
                stopWatch.Start();
                
                stopwtachStateTextBox.Text = " Pause";
                stopwatchStateIcon.Symbol = Symbol.Pause;
            }
        }

        private void stopwatchToggleButton_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopWatch.Stop();
            stopwtachStateTextBox.Text = " Start";
            stopwatchStateIcon.Symbol = Symbol.Play;
        }

        private void stopwatchResetButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopwatchToggleButton.IsChecked = false;
            stopwatchTime.Text = "00:00:00.0000000";
            stopWatch.Reset();
            aTimer.Dispose();
            
        }
    }
}
