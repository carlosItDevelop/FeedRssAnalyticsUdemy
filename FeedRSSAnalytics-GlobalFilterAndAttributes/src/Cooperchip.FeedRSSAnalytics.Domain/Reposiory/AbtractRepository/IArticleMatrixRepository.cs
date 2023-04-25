using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Services;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository
{
    public interface IArticleMatrixRepository
    {
        IQueryable<Category> GetDistinctCategory();

        Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndOrTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null);

        Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null);

        Task<PagedResulFeed<ArticleMatrix>> GetFilterByYear(int pageIndex, int pageSize, int? query = null);
    }
}
