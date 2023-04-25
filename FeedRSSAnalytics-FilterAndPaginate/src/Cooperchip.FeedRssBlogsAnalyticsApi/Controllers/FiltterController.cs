using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FiltterController : ControllerBase
    {
        private readonly IArticleMatrixRepository _articleMatrixRepository;

        public FiltterController(IArticleMatrixRepository articleMatrixRepository)
        {
            _articleMatrixRepository = articleMatrixRepository;
        }

        [HttpGet("GetDistinctCategory")]
        public IQueryable<Category> GetDistinctCategory()
        {
            return _articleMatrixRepository.GetDistinctCategory();
        }


        [HttpGet]
        [Route("GetCategoryAndOrTitle")]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResulFeed<ArticleMatrix>>> GetCategoryAndOrTitle(
                                       [FromQuery] int pi, int ps, string? q=null, string? t=null) 
        {
            var artigos = await _articleMatrixRepository.GetCategoryAndOrTitle(pi, ps, q, t);

            var metadata = new
            {
                artigos.TotalPages,
                artigos.PageIndex,
                artigos.PageSize,
                artigos.HasPrevious,
                artigos.HasNext,
                artigos.TotalResults
            };

            Response.Headers.Add("Content-Type", "application/json");
            Response.Headers.Add("x-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("x-Pagination-Result", $"Retornando {artigos.TotalResults} Registros do Banco de Dads");
            Response.Headers.Add("x-Pagination-Result-data", $"{artigos.Query}");


            return Ok(artigos);
        }

    }
}
