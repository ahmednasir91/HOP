using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using HomeOnPhone.ViewModels;
using Newtonsoft.Json;
using ProtoBuf;

namespace HomeOnPhone
{
    public partial class SellHomePreview
    {
        private SellHomeViewModel viewModel;
        public SellHomePreview()
        {
            InitializeComponent();

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            const string FileName = "HOPSO.bin";
            try
            {
                using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!myIsolatedStorage.FileExists(FileName))
                        NavigationService.Navigate(new Uri("/SellHome.xaml"));
                    using (var fileStream = myIsolatedStorage.OpenFile(FileName, FileMode.Open, FileAccess.Read))
                    {
                        viewModel = Serializer.Deserialize<SellHomeViewModel>(fileStream);
                        viewModel.BitmapImage = !String.IsNullOrEmpty(viewModel.ImageFileName)
                                                    ? ImageFromBuffer(viewModel.ImageBytes)
                                                    : new BitmapImage(new Uri("/Images/nophoto_church.png",
                                                                              UriKind.Relative));
                        DataContext = viewModel;
                    }
                    myIsolatedStorage.DeleteFile(FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.SetSource(stream);
            return image;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(viewModel);
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/json";
            try
            {
                Save.IsEnabled = false;
                await webClient.UploadStringTaskAsync(new Uri("http://hopapiservice.azurewebsites.net/api/Hop", UriKind.Absolute), "POST", json);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to submit.");
            }
            finally
            {
                Save.IsEnabled = true;
            }
            
            MessageBox.Show("Added Successfully.");
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}