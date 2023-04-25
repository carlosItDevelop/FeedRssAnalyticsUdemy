using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;

namespace Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository
{
    public class ArticleMatrixRepository : IArticleMatrixRepository
    {

        private readonly ApplicationDbContext _context;

        public ArticleMatrixRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetDistinctCategory()
        {
            return from x in _context.ArticleMatrices?.GroupBy(x => x.Category)
                   select new Category
                   {
                       Name = x.FirstOrDefault().Category,
                       Count = x.Count()
                   };
        }
    }
}
