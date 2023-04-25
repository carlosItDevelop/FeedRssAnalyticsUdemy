using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorHelpers
{
    public static class ResultLikes
    {
        //public static HtmlNode GenerateLikes(HtmlDocument htmlDocument, ArticleMatrix articleMatrix)
        //{
        //    var like = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='LabelLikeCount']");
        //    if (like != null)
        //    {
        //        _ = int.TryParse(like.InnerText, out int likes);
        //        articleMatrix.Likes = likes;

        //    }
        //    return like;
        //}

        public static int GenerateLikes(HtmlDocument htmlDocument, ArticleMatrix articleMatrix)
        {
            var like = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='LabelLikeCount']");
            int likes = 0;
            if (like != null)
            {
                _ = int.TryParse(like.InnerText, out likes);
                articleMatrix.Likes = likes;
            }
            return likes;
        }


    }

}
