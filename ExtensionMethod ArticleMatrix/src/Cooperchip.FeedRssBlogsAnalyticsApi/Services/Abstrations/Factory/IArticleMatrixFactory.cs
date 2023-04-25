using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory
{
    public interface IArticleMatrixFactory
    {
        Task<ArticleMatrix> CreateArticleMatrix(string authorId, Feed feed, HtmlDocument htmlDocument);
    }
}
