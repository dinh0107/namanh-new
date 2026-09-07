using hailinh.Models;
using System.Data.Entity;

namespace hailinh.DAL
{
    public class DataEntities : DbContext
    {
        public DataEntities() : base("name=DataEntities")
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ConfigSite> ConfigSites { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Introduce> Introduces { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CarService> CarServices { get; set; }
        public DbSet<CarServiceDetail>  CarServiceDetails { get; set; }
        public DbSet<CarServicePrice> CarServicePrices { get; set; }
        public DbSet<PriceTable> PriceTables { get; set; }
        public DbSet<PriceLangding> PriceLangdings { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
