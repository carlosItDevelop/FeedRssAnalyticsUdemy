using Cooperchip.FeedRSSAnalytics.Domain.Entities;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory
{
    public interface IArticleMatrixFactory
    {
        Task<ArticleMatrix> CreateArticleMatrix(string authorId, Feed feed);
    }
}
