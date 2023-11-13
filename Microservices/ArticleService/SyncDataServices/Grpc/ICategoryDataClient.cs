using System.Collections.Generic;
using ArticleService.Models;

namespace ArticleService.SyncDataServices.Grpc
{
    public interface ICategoryDataClient
    {
        IEnumerable<Category> ReturnAllCategories();
    }
}