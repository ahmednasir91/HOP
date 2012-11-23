using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace HomeOnPhone.ViewModels
{
    public class HomeDetails
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public int SqFt { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public string Owner { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Price { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ImageFileName { get; set; }
        public int Index { get; set; }
        public int TestField { get; set; }
        public int TestField2 { get; set; }

        public BitmapImage Image
        {
            get
            {
                
                var bitmap = new BitmapImage();
                if(ImageBytes != null)
                    bitmap.SetSource(new MemoryStream(ImageBytes));
                else
                    bitmap.UriSource = new Uri("/Images/nophoto_church.png", UriKind.Relative);
                return bitmap;
            }
        }
    }
}
