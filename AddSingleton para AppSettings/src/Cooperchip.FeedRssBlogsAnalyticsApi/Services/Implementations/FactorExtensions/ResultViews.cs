using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorExtensions
{
    public static class ResultViews
    {
        public static ArticleMatrix GenerateViews(this ArticleMatrix articleMatrix, HtmlDocument htmlDocument)
        {
            var factory = articleMatrix;

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

            factory.Views = articleMatrix.Views;
            return factory;

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


