using ContactAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                   
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<User> Users{ get; set; }
    }
}
