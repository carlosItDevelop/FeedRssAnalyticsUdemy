using AutoMapper;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Automappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Authors, AuthorsDto>().ReverseMap();
            CreateMap<Feed, FeedDto>().ReverseMap();
            CreateMap<ArticleMatrix, ArticleMatrixDto>().ReverseMap();
        }
    }
}
