using CategoryService.Models;

namespace CategoryService.Data
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void Create(Category obj);
    }
}