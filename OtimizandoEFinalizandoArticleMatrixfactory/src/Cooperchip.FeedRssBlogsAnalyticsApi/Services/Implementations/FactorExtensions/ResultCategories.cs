using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions
{
    public static class ResultCategories
    {
        public static ArticleMatrix GenerateCategory(this ArticleMatrix factory, HtmlDocument htmlDocument)
        {
            string category = "Videos";
            if (htmlDocument.GetElementbyId("ImgCategory") != null)
            {
                category = htmlDocument.GetElementbyId("ImgCategory").GetAttributeValue("title", "");
            }

            factory.Category = category;

            return factory;
        }
    }
}
