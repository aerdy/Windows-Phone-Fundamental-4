using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;

namespace WPTestApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        //runs on the UI Thread
        private DispatcherTimer dispatcherTimer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.EndTime = DateTime.MinValue;
            base.OnNavigatedFrom(e);
        }

        private DateTime EndTime { get; set; }

        private void btnStartClick(object sender, RoutedEventArgs e)
        {
            if (this.dispatcherTimer == null)
            {
                this.dispatcherTimer = new DispatcherTimer();
                this.dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
                this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            }

            if (this.EndTime == DateTime.MinValue)
            {
                this.EndTime = DateTime.Now + (TimeSpan)this.timeSpan.Value;
            }
            
            this.dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var remaining = this.EndTime - DateTime.Now;
            int remainingSeconds = (int)remaining.TotalSeconds;
            this.timeSpan.Value = TimeSpan.FromSeconds(remainingSeconds);

            if (remaining.TotalSeconds <= 0)
            {
                this.dispatcherTimer.Stop();
            }
        }

        private void btnStopClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherTimer.Stop();
            this.EndTime = DateTime.MinValue;
            this.timeSpan.Value = TimeSpan.FromSeconds(0);
        }

        private void btnPauseClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherTimer.Stop();
        }
    }
}