using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Microsoft.EntityFrameworkCore;

namespace Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository
{
    public class QueryRepository : IQueryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QueryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Authors>? GetAuthors()
        {
            return _dbContext.ArticleMatrices?.GroupBy(author => author.AuthorId)
                  .Select(group =>
                        new Authors
                        {
                            AuthorId = group.FirstOrDefault().AuthorId,
                            Author = group.FirstOrDefault().Author,
                            Count = group.Count()
                        })
                  .OrderBy(group => group.Author);
        }


        public async Task<List<Category>> GetCategoriesByAuthorId(string authorId)
        {
            var retval = await _dbContext.ArticleMatrices
                .Where(x => x.AuthorId == authorId)
                .GroupBy(x => x.Category)
                .Select(group => new Category
                {
                    Name = group.FirstOrDefault().Category,
                    Count = group.Count()
                }).ToListAsync();

            return retval;
        }
    }
}
