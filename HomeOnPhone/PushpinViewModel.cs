using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace HomeOnPhone
{
    public class PushpinViewModel : INotifyPropertyChanged
    {
        
        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private string _address;
        private BitmapImage _image;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set { _price = value;
            OnPropertyChanged("Price");}
        }

        #region INotifyMembers

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}