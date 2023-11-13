using System;
using System.Collections.Generic;
using System.Linq;

using ArticleService.Data;
using ArticleService.Models;

namespace ArticleService.Data
{
    public class ArticleRepo : IArticleRepo
    {
        private readonly AppDbContext _context;

        public ArticleRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCategory(Category plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Categories.Add(plat);
        }

        public bool ExternalCategoryExists(int externalCategoryId)
        {
            return _context.Categories.Any(p => p.ExternalID == externalCategoryId);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public bool CategoryExits(int categoryId)
        {
            return _context.Categories.Any(p => p.Id == categoryId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Article> GetArticlesForCategory(int categoryId)
        {
            return _context.Articles
                .Where(c => c.CategoryId == categoryId)
                .OrderBy(c => c.Category.Title);
        }

        public Article GetArticle(int categoryId, int topicId)
        {
            return _context.Articles
                .Where(c => c.CategoryId == categoryId && c.Id == topicId).FirstOrDefault();
        }

        public void CreateArticle(int categoryId, Article topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            topic.CategoryId = categoryId;
            _context.Articles.Add(topic);
        }
    }

}