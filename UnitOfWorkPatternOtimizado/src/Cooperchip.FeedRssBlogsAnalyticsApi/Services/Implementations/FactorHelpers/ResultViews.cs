using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorHelpers
{
    public static class ResultViews
    {
        public static string GenerateViews(HtmlDocument htmlDocument, ArticleMatrix articleMatrix)
        {
            #region: Old version
            //var view = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='ViewCounts']");
            //if (view != null)
            //{
            //    articleMatrix.Views = view.InnerText;

            //    if (articleMatrix.Views.Contains('m'))
            //    {
            //        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
            //    }
            //    else if (articleMatrix.Views.Contains('k'))
            //    {
            //        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
            //    }
            //    else
            //    {
            //        _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
            //        articleMatrix.ViewsCount = viewCount;
            //    }
            //}
            //else
            //{
            //    var newsView = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='spanNewsViews']");
            //    if (newsView != null)
            //    {
            //        articleMatrix.Views = newsView.InnerText;

            //        if (articleMatrix.Views.Contains('m'))
            //        {
            //            articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
            //        }
            //        else if (articleMatrix.Views.Contains('k'))
            //        {
            //            articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
            //        }
            //        else
            //        {
            //            _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
            //            articleMatrix.ViewsCount = viewCount;
            //        }
            //    }
            //    else
            //    {
            //        articleMatrix.ViewsCount = 0;
            //    }
            //}

            //return view;
            #endregion

            var view = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='ViewCounts']") ??
                       htmlDocument.DocumentNode.SelectSingleNode("//span[@id='spanNewsViews']");

            if (view != null)
            {
                articleMatrix.Views = view.InnerText;
                articleMatrix.ViewsCount = CalculateViewCount(articleMatrix.Views);
            }
            else
            {
                articleMatrix.ViewsCount = 0;
                articleMatrix.Views = "0";
            }
            return articleMatrix.Views;

        }

        private static decimal CalculateViewCount(string viewCountString)
        {
            var suffixToMultiplier = new Dictionary<char, decimal>
            {
                { 'm', 1000000 },
                { 'k', 1000 }
            };

            if (decimal.TryParse(viewCountString, out decimal viewCount))
            {
                return viewCount;
            }

            foreach (var entry in suffixToMultiplier)
            {
                if (viewCountString.EndsWith(entry.Key) && decimal.TryParse(viewCountString[0..^1], out decimal parsedViewCount))
                {
                    return parsedViewCount * entry.Value;
                }
            }

            return 0;
        }


    }
}


