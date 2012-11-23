using System;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HomeOnPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;

namespace HomeOnPhone
{
    public partial class SellHome
    {
        private GeoCoordinateWatcher _watcher = null;
        public GeoCoordinate MostRecentPosition { get; set; }
        private const string _requestUri = @"https://maps.googleapis.com/maps/api/geocode/json?{0}={1}&sensor=false";
        private readonly SellHomeViewModel viewModel;
        public SellHome()
        {
            StartWatcher();
            InitializeComponent();
            viewModel = new SellHomeViewModel();
            DataContext = viewModel;
            viewModel.PropertyChanged += viewModel_PropertyChanged;
        }


        void StartWatcher()
        {
            if (_watcher == null)
            {
                _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                _watcher.PositionChanged += watcher_PositionChanged;
            }
            _watcher.Start();
            if (_watcher.Permission != GeoPositionPermission.Denied) return;
            MessageBox.Show("Location services are disabled, using Karachi as the default Location.");
            MostRecentPosition = new GeoCoordinate(24.895079, 67.028433);
        }
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            MostRecentPosition = e.Position.Location;
        }


        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Location":
                    if (!String.IsNullOrEmpty(viewModel.Location))
                        SetLocation(viewModel.Location);
                    else
                    {
                        MessageBox.Show("Please Enter a Location", "Error", MessageBoxButton.OK);
                    }
                    break;
            }
        }


        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {
            googlemap.ZoomLevel++;
        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {
            googlemap.ZoomLevel--;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            SetLocation("Karachi");
            StreetMode.IsChecked = true;
            hybrid.Visibility = Visibility.Collapsed;
            satellite.Visibility = Visibility.Collapsed;
            street.Visibility = Visibility.Visible;
            physical.Visibility = Visibility.Collapsed;
            wateroverlay.Visibility = Visibility.Collapsed;
            
        }

        private async void SetLocation(string address, GeoCoordinate coordinate = null)
        {
            WebClient client = new WebClient();
            if (coordinate == null)
            {
                client.DownloadStringCompleted += UpdateLocation;
                client.Headers["Accept"] = "application/json";
                client.DownloadStringAsync(new Uri(String.Format(_requestUri, "address", address), UriKind.Absolute));
            }
            else
            {
                var uri = String.Format(_requestUri, "latlng", coordinate.Latitude + "," + coordinate.Longitude);
                var response =
                    await
                    client.DownloadStringTaskAsync(uri);
                var json = JObject.Parse(response);
                if (String.Equals((string)json.SelectToken("status"), String.Empty))
                {
                    MessageBox.Show("An error occured", (string)json.SelectToken("status") ?? "Error", MessageBoxButton.OK);
                    return;
                }
                viewModel.PropertyChanged -= viewModel_PropertyChanged;
                viewModel.Location = (string)json.SelectToken("results[0].formatted_address");
                viewModel.PropertyChanged += viewModel_PropertyChanged;
                googlemap.Center = coordinate;
                Pushpin pushpin = new Pushpin
                {
                    Location = coordinate,
                    Template = Application.Current.Resources["PushpinControlCurrentTemplate"] as ControlTemplate,
                    Content = "Me"
                };
                if (googlemap.Children.Any(p => p is Pushpin))
                    googlemap.Children.Remove(googlemap.Children.Single(p => p is Pushpin));
                googlemap.Children.Add(pushpin);
                googlemap.ZoomLevel = 16;
                EnableProceedButton();
            }
        }

        private void EnableProceedButton(bool show = true)
        {
            (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = show;
        }

        private void UpdateLocation(object sender, DownloadStringCompletedEventArgs e)
        {
            var json = JObject.Parse(e.Result);
            if (String.Equals((string)json.SelectToken("status"), String.Empty))
            {
                MessageBox.Show("An error occured", (string)json.SelectToken("status") ?? "Error", MessageBoxButton.OK);
                return;
            }
            if (String.Equals((string)json.SelectToken("status"), "ZERO_RESULTS"))
            {
                MessageBox.Show("Address not found. Please try again.", (string)json.SelectToken("status") ?? "Error", MessageBoxButton.OK);
                return;
            }
            viewModel.PropertyChanged -= viewModel_PropertyChanged;
            viewModel.Location = (string)json.SelectToken("results[0].formatted_address");
            viewModel.PropertyChanged += viewModel_PropertyChanged;
            googlemap.Center = new GeoCoordinate((double)json.SelectToken("results[0].geometry.location.lat"),
                                                 (double)json.SelectToken("results[0].geometry.location.lng"));
            googlemap.ZoomLevel = 14;
        }

        private void MapMode_Checked(object sender, RoutedEventArgs e)
        {
            if (HybirdMode.IsChecked == true)
            {
                hybrid.Visibility = Visibility.Visible;
                satellite.Visibility = Visibility.Collapsed;
                street.Visibility = Visibility.Collapsed;
                physical.Visibility = Visibility.Collapsed;
                wateroverlay.Visibility = Visibility.Collapsed;
            }

            else if (SatelliteMode.IsChecked == true)
            {
                hybrid.Visibility = Visibility.Collapsed;
                satellite.Visibility = Visibility.Visible;
                street.Visibility = Visibility.Collapsed;
                physical.Visibility = Visibility.Collapsed;
                wateroverlay.Visibility = Visibility.Collapsed;
            }
            else if (StreetMode.IsChecked == true)
            {
                hybrid.Visibility = Visibility.Collapsed;
                satellite.Visibility = Visibility.Collapsed;
                street.Visibility = Visibility.Visible;
                physical.Visibility = Visibility.Collapsed;
                wateroverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void GestureListener_Hold(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            var pushpinLocation = googlemap.ViewportPointToLocation(e.GetPosition(googlemap));
            Pushpin pushpin = new Pushpin
                                  {
                                      Location = pushpinLocation,
                                      Template = Application.Current.Resources["PushpinControlTemplate"] as ControlTemplate,
                                      Content = "1"
                                  };
            if (googlemap.Children.Any(p => p is Pushpin))
                googlemap.Children.Remove(googlemap.Children.Single(p => p is Pushpin));
            googlemap.Children.Add(pushpin);
            EnableProceedButton();
        }

        private async void ProceedButton_Click(object sender, EventArgs e)
        {
            var coordinate = (googlemap.Children.Single(p => p is Pushpin) as Pushpin).Location;
            var address = await GetAddress(coordinate);
            if (String.IsNullOrEmpty(address))
            {
                MessageBox.Show("Unable to get the location of the pin. Please try again.");
                googlemap.Children.Remove(googlemap.Children.Single(p => p is Pushpin));
                EnableProceedButton(false);
                return;
            }
            var queryString = String.Format("?Location={0}&Latitude={1}&Longitude={2}&Address={3}", viewModel.Location,
                                            coordinate.Latitude, coordinate.Longitude, address);
            NavigationService.Navigate(new Uri("/SellHomeSec.xaml" + queryString, UriKind.Relative));
        }

        private async Task<string> GetAddress(GeoCoordinate coordinate)
        {
            var data =
                await
                new WebClient().DownloadStringTaskAsync(new Uri(String.Format(_requestUri, "latlng",
                                                                  coordinate.Latitude + "," + coordinate.Longitude)));
            return await TaskEx.Run(() =>
                                        {
                                            var json = JObject.Parse(data);
                                            if (!String.Equals((string)json.SelectToken("status"), "OK"))
                                                return "Error " + (string)json.SelectToken("status");
                                            return (string)json.SelectToken("results[0].formatted_address");
                                        });
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            SetLocation(String.Empty, MostRecentPosition);
        }
    }
}