using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

        public DbSet<Product> Module1 { get; set; }
    }
}
