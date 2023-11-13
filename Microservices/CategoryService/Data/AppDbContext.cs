using CategoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions opt):base(opt)
        {            
        }

        public DbSet<Category> Categories { get; set; } 
    }
}
