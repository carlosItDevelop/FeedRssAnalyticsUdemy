using Cooperchip.FeedRSSAnalytics.CoreShare.Configurations;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations;
using System.Globalization;
using System.Xml.Linq;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations
{
    public class FeedProcessor : IFeedProcessor
    {
        private readonly CultureInfo _culture;

        public FeedProcessor()
        {
            _culture = new CultureInfo("en-US");
        }

        public async Task<IEnumerable<Feed>> ProcessorFeed(XDocument doc, AppSettings appSettings)
        {
            var entries = from item in doc.Descendants().
                First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                          select new Feed
                          {
                              Content = item.Elements().First(i => i.Name.LocalName == "description").Value,

                              Link = (item.Elements().First(i => i.Name.LocalName == "link").Value).StartsWith("/")
                                  ? $"{appSettings.BaseUrl}" + item.Elements().First(i => i.Name.LocalName == "link").Value
                                  : item.Elements().First(i => i.Name.LocalName == "link").Value,

                              PubDate = Convert.ToDateTime(item.Elements().First(i => i.Name.LocalName == "pubDate").Value, _culture),

                              Title = item.Elements().First(i => i.Name.LocalName == "title").Value,

                              FeedType = (item.Elements().First(i => i.Name.LocalName == "link").Value).ToLowerInvariant().Contains("blog")
                                  ? "Blog"
                                  : (item.Elements().First(i => i.Name.LocalName == "link").Value).ToLowerInvariant().Contains("news")
                                      ? "News"
                                      : "Article",

                              Author = item.Elements().First(i => i.Name.LocalName == "author").Value
                          };
            return await Task.FromResult(entries);
        }
    }
}
