using Cooperchip.FeedRSSAnalytics.CoreShare.Configurations;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using System.Xml.Linq;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations
{
    public interface IFeedProcessor
    {
        Task<IEnumerable<Feed>> ProcessorFeed(XDocument doc, AppSettings appSettings);
    }
}
