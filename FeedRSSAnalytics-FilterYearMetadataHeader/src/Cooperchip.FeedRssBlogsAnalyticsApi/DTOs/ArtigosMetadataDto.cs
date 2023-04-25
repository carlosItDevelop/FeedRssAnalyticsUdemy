using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Services;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.DTOs
{
    public class ArtigosMetadataDto
    {
        public int TotalResults { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }

        public ArtigosMetadataDto(PagedResulFeed<ArticleMatrix> artigos)
        {
            TotalResults = artigos.TotalResults;
            PageIndex = artigos.PageIndex;
            PageSize = artigos.PageSize;
            TotalPages = artigos.TotalPages;
            HasPrevious = artigos.HasPrevious;
            HasNext = artigos.HasNext;
        }
    }
}
