using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW;

namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.GenericAbstractions
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
