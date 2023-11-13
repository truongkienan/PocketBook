using System;
using System.Collections.Generic;
using AutoMapper;
using ArticleService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using CategoryService;

namespace ArticleService.SyncDataServices.Grpc
{
    public class CategoryDataClient : ICategoryDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoryDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Category> ReturnAllCategories()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCategory"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcCategory"]);
            var client = new GrpcCategory.GrpcCategoryClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCategories(request);
                Console.WriteLine("--> Connect to GRPC server successfully");
                return _mapper.Map<IEnumerable<Category>>(reply.Category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}