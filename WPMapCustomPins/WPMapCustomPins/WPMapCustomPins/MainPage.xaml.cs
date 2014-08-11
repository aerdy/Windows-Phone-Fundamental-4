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
using Microsoft.Phone.Controls.Maps;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace WPMapCustomPins
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        GeoCoordinateWatcher watcher;
        private void startLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20;
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
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
                MessageBox.Show("Please wait while your prosition is determined....");
                return;
            }

            this.map.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if (this.map.Children.Count != 0)
            {
                var pushpin = map.Children.FirstOrDefault(p => (p.GetType() == typeof(Pushpin) && ((Pushpin)p).Tag == "locationPushpin"));

                if (pushpin != null)
                {
                    this.map.Children.Remove(pushpin);
                }
            }

            Pushpin locationPushpin = new Pushpin();
            //locationPushpin.Style = this.Resources["PushpinStyle"] as Style;
           // locationPushpin.Template = this.Resources["PinTemplate"] as ControlTemplate;
          //  locationPushpin.Template = this.Resources["ImagePin"] as ControlTemplate;


            //Uri imgUri = new Uri("Images/MapPin2.png", UriKind.RelativeOrAbsolute);
            //BitmapImage imgSourceR = new BitmapImage(imgUri);
            //ImageBrush imgBrush = new ImageBrush() { ImageSource = imgSourceR };
           
            //locationPushpin.Content = new Rectangle()
            //{
            //    Fill = imgBrush,
            //    Height = 64,
            //    Width = 64
            //};

            locationPushpin.Background = new SolidColorBrush(Colors.Purple);
            locationPushpin.Content = "You are here";
            locationPushpin.Tag = "locationPushpin";
            locationPushpin.Location = watcher.Position.Location;
            this.map.Children.Add(locationPushpin);
            this.map.SetView(watcher.Position.Location, 18.0);
        }
    }
}