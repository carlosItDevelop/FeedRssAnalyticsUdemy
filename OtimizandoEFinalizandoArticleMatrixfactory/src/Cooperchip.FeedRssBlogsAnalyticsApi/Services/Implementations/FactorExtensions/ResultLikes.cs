using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions
{
    public static class ResultLikes
    {
        public static ArticleMatrix GenerateLikes(this ArticleMatrix articleMatrix, HtmlDocument htmlDocument)
        {
            var factory = articleMatrix;
            var like = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='LabelLikeCount']");
            int likes = 0;
            if (like != null)
            {
                _ = int.TryParse(like.InnerText, out likes);
                articleMatrix.Likes = likes;
            }
            factory.Likes = likes;
            return factory;
        }
    }
}
