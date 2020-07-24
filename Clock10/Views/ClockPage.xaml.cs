using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;



namespace Clock10.Views
{
    public sealed partial class ClockPage : Page, INotifyPropertyChanged
    {
        Timer t = new Timer();


        public ClockPage()
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

        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        int timesTicked = 1;

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //IsEnabled defaults to false
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;
            //Time since last tick should be very very close to Interval
            timesTicked++;
            localWait.Visibility = Visibility.Collapsed;
            localTime.Text = Convert.ToString(time).Substring(11 ,9);
            utcTime.Text = DateTime.UtcNow.ToString().Substring(11, 8);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimerSetup();
            DateTime thisDay = DateTime.Today;
            DateTime utcDay = DateTime.UtcNow.Date;
            localDate.Text = thisDay.ToString("D") + "  (Local)";
            utcDate.Text = utcDay.ToString("D") + "  (UTC)";
        }
    }
}
