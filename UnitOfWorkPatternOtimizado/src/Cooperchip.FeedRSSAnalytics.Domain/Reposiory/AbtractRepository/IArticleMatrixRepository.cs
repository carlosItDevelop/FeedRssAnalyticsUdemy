using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.GenericAbstractions;
using Cooperchip.FeedRSSAnalytics.Domain.Services;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository
{
    public interface IArticleMatrixRepository : IGenericRepository<ArticleMatrix>
    {
        // ===/ Queries ====
        IQueryable<Category> GetDistinctCategory();
        Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndOrTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null);
        Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null);
        Task<PagedResulFeed<ArticleMatrix>> GetFilterByYear(int pageIndex, int pageSize, int? query = null);
        // ===/ Commands ===
        Task RemoveByAuthorIdAsync(string? authorId);
        Task AddArticlematrixAsync(IEnumerable<ArticleMatrix> articleMatrices);
    }
}
