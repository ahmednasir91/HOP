using System;
using System.Data.Entity;
using System.Linq;
namespace HOPApiService.Models
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
        public double Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TestField { get; set; }
        public int TestField2 { get; set; }
    }

    public class SamplePage
    {
        public string WhereAmI { get; set; }
    }
    public class DataContext : DbContext
    {
        public DbSet<HOP> Hops { get; set; }
        public DbSet<SamplePage> SamplePages { get; set; }
    }



    public class HOPRepository
    {
        private readonly DataContext _db;

        public HOPRepository()
        {
            _db = new DataContext();
        }

        public IQueryable<HOP> GetAll()
        {
            return _db.Hops;
        }

        public IQueryable<SamplePage> GetPages()
        {
            return _db.SamplePages;
        }
    }
}