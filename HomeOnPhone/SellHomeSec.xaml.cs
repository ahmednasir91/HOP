using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HomeOnPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using ProtoBuf;

namespace HomeOnPhone
{
    public partial class SellHomeSec : PhoneApplicationPage
    {
        public SellHomeViewModel viewModel;
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var data = NavigationContext.QueryString;
            viewModel.Location = data["Location"];
            viewModel.Longitude = Convert.ToDouble(data["Longitude"]);
            viewModel.Latitude = Convert.ToDouble(data["Latitude"]);
            viewModel.Address = data["Address"];
            base.OnNavigatedTo(e);
        }
        public SellHomeSec()
        {
            InitializeComponent();
            viewModel = new SellHomeViewModel();
            DataContext = viewModel;
            viewModel.DateAdded = DateTime.Now;
            viewModel.ErrorsChanged += ViewModelOnErrorsChanged;
        }

        private void ViewModelOnErrorsChanged(object sender, DataErrorsChangedEventArgs dataErrorsChangedEventArgs)
        {
            var error = dataErrorsChangedEventArgs.PropertyName;

        }

        private void CameraButton_Click(object sender, EventArgs e)
        {
            var _cameraTask = new CameraCaptureTask();
            _cameraTask.Completed += OnCameraAlbumTaskOnCompleted;
            _cameraTask.Show();
        }

        private void OnCameraAlbumTaskOnCompleted(object o, PhotoResult result)
        {
            if (result.TaskResult == TaskResult.OK)
            {
                viewModel.ImageBytes = new byte[result.ChosenPhoto.Length];
                result.ChosenPhoto.Read(viewModel.ImageBytes, 0, viewModel.ImageBytes.Length);
                viewModel.ImageFileName = result.OriginalFileName;
            }
            else
            {
                viewModel.ImageFileName = String.Empty;
            }
        }

        private void AlbumButton_Click(object sender, EventArgs e)
        {
            var _photoTask = new PhotoChooserTask();
            _photoTask.Completed += OnCameraAlbumTaskOnCompleted;
            _photoTask.Show();
        }

        private void MapsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SellHome.xaml", UriKind.Relative));
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if(!ValidatePage())
            {
                MessageBox.Show("There are errors in the form. Please correct the fields that are marked red.");
                return;
            }

            const string FileName = "HOPSO.bin";
            try
            {
               using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(FileName))
                        myIsolatedStorage.DeleteFile(FileName);
                    using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
                    {
                        Serializer.Serialize(fileStream, viewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            NavigationService.Navigate(new Uri("/SellHomePreview.xaml", UriKind.Relative));
        }

        private bool ValidatePage()
        {
            viewModel.Owner = viewModel.Owner;
            viewModel.Price = viewModel.Price;
            viewModel.SqFt = viewModel.SqFt;
            viewModel.Beds = viewModel.Beds;
            viewModel.Baths = viewModel.Baths;
            viewModel.EmailAddress = viewModel.EmailAddress;
            return !viewModel.HasErrors;
        }

        private void TextBox_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                (sender as TextBox).BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                (sender as TextBox).BorderBrush = new SolidColorBrush(Colors.Green);
            }
        }
    }
}