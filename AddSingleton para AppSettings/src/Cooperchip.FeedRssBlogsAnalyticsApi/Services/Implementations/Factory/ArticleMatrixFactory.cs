using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.Factory
{
    public class ArticleMatrixFactory : IArticleMatrixFactory
    {
        public async Task<ArticleMatrix> CreateArticleMatrix(string authorId, Feed feed, HtmlDocument htmlDocument)
        {
            var articleMatrix = new ArticleMatrix
            {
                AuthorId = authorId,
                Author = feed.Author,
                Type = feed.FeedType,
                Link = feed.Link,
                Title = feed.Title,
                PubDate = feed.PubDate
            };

            articleMatrix
                .GenerateCategory(htmlDocument)
                .GenerateLikes(htmlDocument)
                .GenerateViews(htmlDocument);

            return await Task.FromResult(articleMatrix);
        }
    }
}
