using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using ProtoBuf;

namespace HomeOnPhone.ViewModels
{
    public enum SellType
    {
        ForSale,
        Rent
    }
    [ProtoContract]
    public class SellHomeViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _location;
        [ProtoMember(2)]
        public string PageTitle { get; set; }
        
        public SellHomeViewModel()
        {
            PageTitle = "sell home";
        }
        [ProtoMember(1)]
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }
        [ProtoMember(3)]
        public double Longitude { get; set; }
        [ProtoMember(4)]
        public double Latitude { get; set; }
        
        private string _address;
        [ProtoMember(5)]
        public string Address
        {
            get { return _address; }
            set
            {
                if (isNotValid("Address", value))
                    return;
                _address = value;
                RaisePropertyChanged("Address");
            }
        }
        
        private string _owner;
        [ProtoMember(6)]
        public string Owner
        {
            get { return _owner; }
            set
            {
                if (isNotValid("Owner", value))
                    return;
                _owner = value;
                RaisePropertyChanged("Owner");
            }
        }
        
        private double _price;
        [ProtoMember(7)]
        public double Price
        {
            get { return _price; }
            set
            {
                if (isNotValid("Price", value))
                    return;
                _price = value;
                RaisePropertyChanged("Price");
            }
        }
        
        private SellType _sellType;
        [ProtoMember(8)]
        public SellType SellType
        {
            get { return _sellType; }
            set
            {
                _sellType = value;
                RaisePropertyChanged("SellType");
            }
        }
        
        private int _beds;
        [ProtoMember(9)]
        public int Beds
        {
            get { return _beds; }
            set
            {
                if (isNotValid("Beds", value))
                    return;
                _beds = value;
                RaisePropertyChanged("Beds");
            }
        }

        private bool isNotValid(string Property, object value)
        {
            var invalid = false;
            switch (Property)
            {
                case "Address":
                case "Owner":
                case "EmailAddress":
                    if (String.IsNullOrEmpty(value as String))
                        AddError(Property, Property + ERROR_REQUIRED, invalid = true);
                    else
                        RemoveError(Property, Property + ERROR_REQUIRED);
                    break;
                case "Beds":
                case "Baths":
                case "Price":
                case "SqFt":
                case "TestField":
                case "TestField2":
                    if(Convert.ToInt32(value) == 0)
                        AddError(Property, Property + ERROR_REQUIRED, invalid = true);
                    else
                        RemoveError(Property, Property + ERROR_REQUIRED);
                    break;
            }
            switch (Property)
            {
                case "Owner":
                    if(value != null && !Regex.IsMatch(value as String, @"^[\p{L} \.\-]+$"))
                        AddError(Property, Property + " not valid.", invalid = true);
                    else
                        RemoveError(Property, Property + " not valid.");
                    break;
   
            }

            return invalid;
        }
        
        private int _baths;
        [ProtoMember(10)]
        public int Baths
        {
            get { return _baths; }
            set
            {
                if (isNotValid("Baths", value))
                    return;
                _baths = value;
                RaisePropertyChanged("Baths");
            }
        }
        
        private int _sqFt;
        [ProtoMember(11)]
        public int SqFt
        {
            get { return _sqFt; }
            set
            {
                if (isNotValid("SqFt", value))
                    return;
                _sqFt = value;
                RaisePropertyChanged("SqFt");
            }
        }
        
        private string _emailAddress;
        [ProtoMember(12)]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (isNotValid("EmailAddress", value))
                    return;
                _emailAddress = value;
                RaisePropertyChanged("EmailAdrress");
            }
        }
        
        private byte[] _imageBytes;
        [ProtoMember(13)]
        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set
            {
                _imageBytes = value;
                RaisePropertyChanged("ImageBytes");
            }
        }
        
        private string _imageFileName;
        [ProtoMember(14)]
        public string ImageFileName
        {
            get { return _imageFileName; }
            set
            {
                _imageFileName = value;
                RaisePropertyChanged("ImageFileName");
            }
        }

        public BitmapImage BitmapImage { get; set; }
        public IEnumerable<SellType> SellTypes
        {
            get { return Enum<SellType>.GetNames().Cast<SellType>(); }
        }

        private DateTime _dateAdded;
        [ProtoMember(15)]
        public DateTime DateAdded
        {
            get { return _dateAdded; }
            set { _dateAdded = value; 
            RaisePropertyChanged("DateAdded");}
        }

        private int _testField;
        [ProtoMember(16)]
        public int TestField
        {
            get { return _testField; }
            set
            {
                _testField = value;
                RaisePropertyChanged("TestField");
            }
        }

        private int _testField2;
        [ProtoMember(17)]
        public int TestField2
        {
            get { return _testField2; }
            set
            {
                _testField2 = value;
                RaisePropertyChanged("TestField2");
            }
        }



        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (propertyName != null && PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo Members

        private const string ERROR_REQUIRED = " cannot be left empty.";


        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning) errors[propertyName].Add(error);
                else errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        // Removes the specified error from the errors collection if it is
        // present. Raises the ErrorsChanged event if the collection changes.
        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) &&
                errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }



        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly Dictionary<String, List<String>> errors = new Dictionary<string, List<string>>();
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        #endregion

        
    }
}

public class Enum<T>
{
    public static IEnumerable<Object> GetNames()
    {

        var type = typeof(T);
        if (!type.IsEnum)
            throw new ArgumentException("Type '" + type.Name + "' is not an enum");

        return (
                   from field in
                       type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                   where field.IsLiteral
                   select Enum.Parse(type, field.Name, true));
    }
}
