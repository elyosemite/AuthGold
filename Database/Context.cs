using AuthGold.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthGold.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {}

        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
