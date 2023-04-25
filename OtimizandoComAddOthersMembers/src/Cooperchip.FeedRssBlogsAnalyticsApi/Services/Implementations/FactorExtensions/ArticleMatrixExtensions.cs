using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorHelpers;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions
{
    public static class ArticleMatrixExtensions
    {

        public static ArticleMatrix AddOthersMembers(this ArticleMatrix factory,
                                                    HtmlDocument htmlDocument)
        {
            factory.Category = ResultCategories.GenerateCategory(htmlDocument);
            factory.Views = ResultViews.GenerateViews(htmlDocument, factory);
            factory.Likes = ResultLikes.GenerateLikes(htmlDocument, factory);

            return factory;
        }

        //public static ArticleMatrix AddCategories(this ArticleMatrix factory,
        //                                            HtmlDocument htmlDocument)
        //{
        //    factory.Category = ResultCategories.GenerateCategory(htmlDocument);
        //    return factory;
        //}

        //public static ArticleMatrix AddViews(this ArticleMatrix factory,
        //                                HtmlDocument htmlDocument)
        //{
        //    factory.Views = ResultViews.GenerateViews(htmlDocument, factory);
        //    return factory;
        //}

        //public static ArticleMatrix AddLikes(this ArticleMatrix factory,
        //                                HtmlDocument htmlDocument)
        //{
        //    factory.Likes = ResultLikes.GenerateLikes(htmlDocument, factory);
        //    return factory;
        //}
    }
}
