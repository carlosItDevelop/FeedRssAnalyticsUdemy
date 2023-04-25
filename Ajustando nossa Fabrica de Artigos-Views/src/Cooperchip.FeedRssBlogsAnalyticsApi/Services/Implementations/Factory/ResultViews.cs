using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using HtmlAgilityPack;
using System.Xml.Linq;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.Factory
{
    public static class ResultViews
    {
        public static HtmlNode GenerateViews(HtmlDocument htmlDocument, ArticleMatrix articleMatrix)
        {
            var view = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='ViewCounts']");
            if (view != null)
            {
                articleMatrix.Views = view.InnerText;

                if (articleMatrix.Views.Contains('m'))
                {
                    articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
                }
                else if (articleMatrix.Views.Contains('k'))
                {
                    articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
                }
                else
                {
                    _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
                    articleMatrix.ViewsCount = viewCount;
                }
            }
            else
            {
                var newsView = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='spanNewsViews']");
                if (newsView != null)
                {
                    articleMatrix.Views = newsView.InnerText;

                    if (articleMatrix.Views.Contains('m'))
                    {
                        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
                    }
                    else if (articleMatrix.Views.Contains('k'))
                    {
                        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
                    }
                    else
                    {
                        _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
                        articleMatrix.ViewsCount = viewCount;
                    }
                }
                else
                {
                    articleMatrix.ViewsCount = 0;
                }
            }

            return view;

        }
    }
}
