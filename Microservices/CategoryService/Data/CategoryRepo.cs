using Microsoft.EntityFrameworkCore;
using CategoryService.Models;

namespace CategoryService.Data
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context=context;
        }
        public void Create(Category obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _context.Categories.Add(obj);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>=0);
        }
    }
}