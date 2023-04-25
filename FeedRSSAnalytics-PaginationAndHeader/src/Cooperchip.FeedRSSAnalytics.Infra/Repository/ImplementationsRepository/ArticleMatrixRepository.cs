using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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



        #region

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
