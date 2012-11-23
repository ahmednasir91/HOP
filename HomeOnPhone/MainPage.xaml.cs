using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HomeOnPhone.Library;
using HomeOnPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System.Linq;
using Telerik.Windows.Controls;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace HomeOnPhone
{
    public partial class MainPage
    {
        private readonly string _apiUrl;
        private readonly MainPageViewModel viewModel;
        private const string _requestUri = @"https://maps.googleapis.com/maps/api/geocode/json?{0}={1}&sensor=false";
        private GeoCoordinateWatcher _watcher = null;
        public GeoCoordinate MostRecentPosition { get; set; }

        public MainPage()
        {
            StartWatcher();
            viewModel = new MainPageViewModel();
            _apiUrl = "http://hopapiservice.azurewebsites.net/api/Hop";
            InitializeComponent();
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
        


        private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        private async void SetLocation(string location, GeoCoordinate coordinate = null)
        {
            WebClient client = new WebClient();
            try
            {
                if (coordinate == null)
                {
                    var uri = String.Format(_requestUri, "address", location);
                    var response =
                        await
                        client.DownloadStringTaskAsync(uri);
                    var json = JObject.Parse(response);
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
                        googlemap.Children.Remove(googlemap.Children.Single(p => (p is Pushpin) && String.Equals((p as Pushpin).Content.ToString(), "Me")));
                    googlemap.Children.Add(pushpin);
                    googlemap.ZoomLevel = 16;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading the location.");
            }

        }

        private void GetCurrentLocation()
        {
            var position = MostRecentPosition;
            if (MostRecentPosition != null)
                SetLocation(null, position);
        }
        private void ToggleProgressBar(bool show = true)
        {
            if(show)
                LayoutRoot.Opacity = 0.2;
            else
            {
                LayoutRoot.Opacity = 1;
            }
            ProgressIndicator progress = new ProgressIndicator
            {
                IsVisible = show,
                IsIndeterminate = true,
                Text = "Downloading..."
            };
            SystemTray.SetProgressIndicator(this, progress);
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleProgressBar();
            GetCurrentLocation();
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers["accepts"] = "application/json";
                var response = await webClient.DownloadStringTaskAsync(_apiUrl);
                var array = JArray.Parse(response);
                foreach (var item in array)
                {
                    viewModel.Homes.Add(item.ToObject<HomeDetails>());
                }
                nearbyList.ItemsSource = GetNearByHomes();
                recentList.ItemsSource = GetRecentHomes();
                AddPushpins(nearbyList.ItemsSource as ObservableCollection<HomeDetails>);
                viewModel.RaisePropertyChanged("isListEmpty");
                ToggleProgressBar(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while loading.");
            }
            finally
            {
                
            }
        }

        private void AddPushpins(IEnumerable<HomeDetails> nearbyHomes)
        {
            foreach (var pushpin in nearbyHomes.Select(h => new { h.Latitude, h.Longitude, h.Index, h.Image, h.Address, h.Price }).
                Select(pushpinData => new Pushpin
                {
                    Template = Application.Current.Resources["PushpinControlTemplate"] as ControlTemplate,
                    Location = new GeoCoordinate(pushpinData.Latitude, pushpinData.Longitude),
                    Content = pushpinData.Index,
                    DataContext = new PushpinViewModel
                                      {
                                          Image = pushpinData.Image,
                                          Address = pushpinData.Address,
                                          Price = pushpinData.Price,
                                      }
                }))
            {

                googlemap.Children.Add(pushpin);
            }
        }

        private ObservableCollection<HomeDetails> GetNearByHomes()
        {
            var keywords = viewModel.Location.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var nearByHomes = viewModel.Homes
                .Where(h => keywords
                                .Any(k => h.Address.Contains(k))).AddIndex();
            return new ObservableCollection<HomeDetails>(nearByHomes.Reverse());
        }


        private ObservableCollection<HomeDetails> GetRecentHomes()
        {
            return new ObservableCollection<HomeDetails>(
                viewModel.Homes.OrderByDescending(h => h.Id).AddIndex());
        }

        private void ButtonZoomOut_Click(object sender, EventArgs eventArgs)
        {
            googlemap.ZoomLevel--;
        }

        private void ButtonZoomIn_Click(object sender, EventArgs eventArgs)
        {
            googlemap.ZoomLevel++;
        }

        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {

        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = null;
                    break;
                case 3:
                    ApplicationBar = ((ApplicationBar)Resources["MapBar"]);
                    break;

                default:
                    ApplicationBar = ((ApplicationBar)Resources["AppBar"]);
                    break;
            }
        }

        private void SellHomeMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SellHome.xaml", UriKind.Relative));
        }

        private void AboutUsMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutUs.xaml", UriKind.Relative));
        }

        private void LocationSearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Focus();
        }

        private void RadHubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                switch ((sender as HubTileBase).Title.ToString())
                {
                    case "Search":
                        PivotPage.SelectedIndex = 3;
                        break;
                    case "Nearby":
                        PivotPage.SelectedIndex = 2;
                        break;
                    case "Sell/Rent":
                        NavigationService.Navigate(new Uri("/SellHome.xaml", UriKind.Relative));
                        break;
                    case "About Us":
                        NavigationService.Navigate(new Uri("/Aboutus.xaml", UriKind.Relative));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured. " + ex.Message);
            }
        }

        private void nearbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = (nearbyList.SelectedValue as HomeDetails).Index;
            var coo = viewModel.Homes.Where(h => h.Index == index).Select(h => new { h.Latitude, h.Longitude }).Single();
            googlemap.Center = new GeoCoordinate(coo.Latitude, coo.Longitude);
            PivotPage.SelectedIndex = 3;
            googlemap.ZoomLevel = 16;
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            SetLocation(null, MostRecentPosition);
        }
    }
}