using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;

namespace Cooperchip.FeedRSSAnalytics.Infra.Repository.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
