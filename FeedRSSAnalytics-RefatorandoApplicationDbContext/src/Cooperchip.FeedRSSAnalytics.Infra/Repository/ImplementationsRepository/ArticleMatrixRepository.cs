using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Microsoft.EntityFrameworkCore;

namespace Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository
{
    public class ArticleMatrixRepository : IArticleMatrixRepository
    {

        private readonly ApplicationDbContext _context;

        public ArticleMatrixRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task RemoveByAuthorIdAsync(string? authorId)
        {
            _context.ArticleMatrices?.RemoveRange(_context.ArticleMatrices.Where(x => x.AuthorId == authorId));
            await _context.SaveChangesAsync();
        }

        public async Task AddArticlematrixAsync(IEnumerable<ArticleMatrix> articleMatrices)
        {
            var newListArticleMatrices = new List<ArticleMatrix>();
            foreach (var item in articleMatrices)
            {
                if (item.Category == "Videos")
                {
                    item.Type = "Video";
                }
                item.Category = item.Category?.Replace("&amp", "&");
                newListArticleMatrices.Add(item);
            };
            await _context.ArticleMatrices.AddRangeAsync(newListArticleMatrices);
            await _context.SaveChangesAsync();
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


        public async Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null)
        {
            var source = _context.ArticleMatrices?.AsQueryable();

            if (!string.IsNullOrEmpty(categoria) || !string.IsNullOrEmpty(title))
            {
                source = source?.Where(x => (categoria == null || x.Category.Contains(categoria)) && (title == null || x.Title.Contains(title)));
            }

            var data = await source.ToListAsync();
            return Paginate(data, pageIndex, pageSize);
        }


        public async Task<PagedResulFeed<ArticleMatrix>> GetCategoryAndOrTitle(int pageIndex, int pageSize, string? categoria = null, string? title = null)
        {
            #region: Algoritmo dos if aninhados
            //var data = new List<ArticleMatrix>();
            //var source = _context.ArticleMatrices?.AsQueryable();

            //if((categoria == null || categoria == string.Empty) && title != null && title != string.Empty)
            //{
            //    data = await source.Where(x => x.Title.Contains(title)).ToListAsync();
            //}
            //else if ((categoria != null && categoria != string.Empty) && title != null && title != string.Empty ) 
            //{
            //    data = await source.Where(x => x.Category.Contains(categoria) && x.Title.Contains(title)).ToListAsync();
            //}else if ((categoria != null & categoria != string.Empty) && title == null || title == string.Empty)
            //{
            //    data = await source.Where(x=>x.Category.Contains(categoria)).ToListAsync();
            //} else
            //{
            //    data = await source.ToListAsync();
            //}
            #endregion

            var source = _context.ArticleMatrices?.AsQueryable();
            if(!string.IsNullOrEmpty(categoria) || !String.IsNullOrEmpty(title)) 
            {
                source = source?.Where(x => (categoria == null || x.Category.Contains(categoria)) && title == null || x.Title.Contains(title));
            }
            var data = await source.ToListAsync();
            return Paginate(data, pageIndex, pageSize);
        }

        public async Task<PagedResulFeed<ArticleMatrix>> GetFilterByYear(int pageIndex, int pageSize, int? query = null)
        {
            IEnumerable<ArticleMatrix>? data = new List<ArticleMatrix>();

            var source = _context?.ArticleMatrices?.AsQueryable();
            
            data = (query != null)
                ? await source.Where(x => x.PubDate.Year >= query).ToListAsync() 
                : await source.ToListAsync();

            return Paginate(data, pageIndex, pageSize);

        }


        #region: Paginate
        private PagedResulFeed<ArticleMatrix> Paginate(IEnumerable<ArticleMatrix> data, int pageIndex, int pageSize)
        {
            int count = data.Count();
            data = data.Skip(pageSize * (pageIndex -1)).Take(pageSize);

            return new PagedResulFeed<ArticleMatrix>
            {
                Data = data,
                TotalResults = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                HasPrevious = pageIndex > 1,
                HasNext = pageIndex < count - 1
            };
        }
        #endregion

    }
}
