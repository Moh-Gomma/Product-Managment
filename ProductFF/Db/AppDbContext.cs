using Microsoft.EntityFrameworkCore;
using ProductFF.Model;

namespace ProductFF.Db
{
    public class AppDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(p => p.Products).HasForeignKey(x => x.CategoryId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
