using System.Collections.Generic;
using ArticleService.Models;

namespace ArticleService.Data
{
    public interface IArticleRepo
    {
        bool SaveChanges();

        // Category
        IEnumerable<Category> GetAllCategories();
        void CreateCategory(Category plat);
        bool CategoryExits(int categoryId);
        bool ExternalCategoryExists(int externalCategoryId);

        // Topic
        IEnumerable<Article> GetArticlesForCategory(int categoryId);
        Article GetArticle(int categoryId, int topicId);
        void CreateArticle(int categoryId, Article topic);
    }
}