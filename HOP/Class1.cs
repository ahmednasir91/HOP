using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HOP
{
    public class HOP
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public int SqFt { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public string Owner { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ImageFileName { get; set; }
    }
}
