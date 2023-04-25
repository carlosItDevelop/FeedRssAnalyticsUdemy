using Cooperchip.FeedRSSAnalytics.Domain.Entities;

namespace Cooperchip.FeedRSSAnalytics.Domain.Services.Abstractions
{
    public interface IArticleMatrixFactory
    {
        Task<ArticleMatrix> CreateArticleMatrix(string authorId, Feed feed);
    }
}
