using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW;

namespace Cooperchip.FeedRSSAnalytics.Domain.Services.Persistences
{
    public class TransactionHandler : ITransactionHandler
    {
        public async Task<bool> PersistirDados(IUnitOfWork uow)
        {
            // Outras regras e Processos acontecendo

            if(await uow.Commit()) return true;

            return false;
        }
    }
}
