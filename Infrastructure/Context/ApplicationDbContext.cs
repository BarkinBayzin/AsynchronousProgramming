using AsynchronousProgramming.Models.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AsynchronousProgramming.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Page> Pages  { get; set; }
        public DbSet<Product> Products  { get; set; }
    }
}
