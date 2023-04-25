namespace Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
    }
}
