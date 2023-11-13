using ArticleService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasMany(p => p.Articles)
                .WithOne(p => p.Category!)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder
                .Entity<Article>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Articles)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
