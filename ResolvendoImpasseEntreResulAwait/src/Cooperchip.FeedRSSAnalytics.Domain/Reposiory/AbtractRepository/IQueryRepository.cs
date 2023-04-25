using Cooperchip.FeedRSSAnalytics.Domain.Entities;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository
{
    public interface IQueryRepository
    {
        Task<List<Category>> GetCategoriesByAuthorId(string authorId);
        IQueryable<Authors> GetAuthors();
    }
}
