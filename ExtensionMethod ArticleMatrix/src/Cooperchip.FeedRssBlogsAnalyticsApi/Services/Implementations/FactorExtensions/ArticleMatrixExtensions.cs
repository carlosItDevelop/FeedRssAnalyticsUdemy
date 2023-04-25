using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorHelpers;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions
{
    public static class ArticleMatrixExtensions
    {
        public static ArticleMatrix AddCategories(this ArticleMatrix factory, HtmlDocument htmlDocument)
        {
            factory.Category = ResultCategories.GenerateCategory(htmlDocument);
            return factory;
        }

        public static HtmlNode AddViews(this ArticleMatrix factory, HtmlDocument htmlDocument)
        {
            var views = ResultViews.GenerateViews(htmlDocument, factory);
            return views;
        }

        public static HtmlNode AddLikes(this ArticleMatrix factory, HtmlDocument htmlDocument)
        {
            var likes = ResultLikes.GenerateLikes(htmlDocument, factory);
            return likes;
        }

    }
}
