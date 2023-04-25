using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Services.Abstractions;

namespace Cooperchip.FeedRSSAnalytics.Domain.Services.Implementations
{
    public class ArticleMatrixFactory : IArticleMatrixFactory
    {
        public async Task<ArticleMatrix> CreateArticleMatrix(string authorId, Feed feed)
        {
            var returnFactory = new ArticleMatrix
            {
                AuthorId = authorId,
                Author = feed.Author,
                Type = feed.FeedType,
                Link = feed.Link,
                Title = feed.Title,
                PubDate = feed.PubDate
            };
            return await Task.FromResult(returnFactory);
        }
    }
}
