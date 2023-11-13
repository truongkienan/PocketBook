using AutoMapper;
using ArticleService.Models;
using ArticleService.Dtos;
using CategoryService;

namespace ArticleService.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            // Source -> Target
            CreateMap<Category, CategoryReadDto>();
            CreateMap<ArticleCreateDto, Article>();
            CreateMap<Article, ArticleReadDto>();
            CreateMap<CategoryPublishedDto, Category>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GrpcCategoryModel, Category>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Articles, opt => opt.Ignore());
        }
    }
}
