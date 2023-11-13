using AutoMapper;
using CategoryService;
using CategoryService.Dtos;
using CategoryService.Models;

namespace PlatformService.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            //  Source -> target
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryReadDto, CategoryPublishedDto>();
            CreateMap<Category, GrpcCategoryModel>();
        }
    }
}