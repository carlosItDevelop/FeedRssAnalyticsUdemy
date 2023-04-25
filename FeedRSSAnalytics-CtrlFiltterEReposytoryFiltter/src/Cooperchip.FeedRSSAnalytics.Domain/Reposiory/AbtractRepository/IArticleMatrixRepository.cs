using Cooperchip.FeedRSSAnalytics.Domain.Entities;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository
{
    public interface IArticleMatrixRepository
    {
        IQueryable<Category> GetDistinctCategory();
    }
}
