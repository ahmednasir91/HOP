using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HomeOnPhone.ViewModels
{


    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            Homes = new List<HomeDetails>();
        }
        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value;
                RaisePropertyChanged("Location");
            }
        }

        public bool isListEmpty
        {
            get { return Homes.Count == 0; }
        }

        public List<HomeDetails> Homes { get; set; }

    #region INotifyMembers
		
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string PropertyName)
        {
            if(!String.IsNullOrEmpty(PropertyName))
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
	#endregion
    }
}
