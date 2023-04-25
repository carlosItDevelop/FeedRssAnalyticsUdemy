using Cooperchip.FeedRSSAnalytics.Domain.Entities;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository
{
    public interface IQueryADORepository
    {
        Task<IEnumerable<Category>> GetCategoriesByAuthorId(string authorId);
        Task<IEnumerable<Authors>> GetAuthors();

        Task<IEnumerable<ArticleMatrix>> GetAllArticlesByAuthorId(string authorId);

    }
}
