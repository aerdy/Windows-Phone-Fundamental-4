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
using System.Device.Location;

namespace WPGPSSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
      
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void startLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20;
                watcher.StatusChanged+= new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
              
            }
            watcher.Start();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show(" The Location Service is working, but it cannot get location data.");
                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                this.notification.Text = "Please wait while your prosition is determined....";
                return;
            }

            List<string> locationData = new List<string>();
            locationData.Add(e.Position.Location.Latitude.ToString("Latitude:" + "0.000"));
            locationData.Add(e.Position.Location.Longitude.ToString("Longitude:" + "0.000"));
            locationData.Add(e.Position.Location.Altitude.ToString());
            locationData.Add(e.Position.Location.Speed.ToString());
            locationData.Add(e.Position.Location.Course.ToString());
            this.locationList.ItemsSource = locationData;
        }

        private void stopLocationButton_Click(object sender, RoutedEventArgs e)
        {
            watcher.Stop();
        }
    }
}