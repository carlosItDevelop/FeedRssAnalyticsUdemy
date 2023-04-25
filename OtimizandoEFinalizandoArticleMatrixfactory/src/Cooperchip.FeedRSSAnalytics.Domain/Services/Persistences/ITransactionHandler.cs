using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW;

namespace Cooperchip.FeedRSSAnalytics.Domain.Services.Persistences
{
    public interface ITransactionHandler
    {
        Task<bool> PersistirDados(IUnitOfWork uow);
    }
}
