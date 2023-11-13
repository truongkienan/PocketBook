using System;
using System.Collections.Generic;
using ArticleService.Models;
using ArticleService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<ICategoryDataClient>();

                var categories = grpcClient.ReturnAllCategories();

                SeedData(serviceScope.ServiceProvider.GetService<IArticleRepo>(), categories);
            }
        }
        
        private static void SeedData(IArticleRepo repo, IEnumerable<Category> categories)
        {
            Console.WriteLine("Seeding new categories...");

            foreach (var item in categories)
            {
                if(!repo.ExternalCategoryExists(item.ExternalID))
                {
                    repo.CreateCategory(item);
                }
                repo.SaveChanges();
            }
        }
    }
}