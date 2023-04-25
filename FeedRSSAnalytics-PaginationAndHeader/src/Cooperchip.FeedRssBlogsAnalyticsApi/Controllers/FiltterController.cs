using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

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
        [Route("GetCategoryAndTitle")]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResulFeed<ArticleMatrix>>> GetCategoryAndTitle(
                                       [FromQuery] int pi = 1, int ps = 10, string? q = null, string? t = null)
        {
            var artigos = await _articleMatrixRepository.GetCategoryAndTitle(pi, ps, q, t);

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
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("X-Pagination-Result", $"Mostrando da pagina [{artigos.PageIndex}] ate a pagina [{artigos.PageSize}], num Total de [{artigos.TotalPages}] paginas e [{artigos.TotalResults}] Registros do Banco de Dados.");


            return Ok(artigos);
        }


        [HttpGet]
        [Route("GetCategoryAndOrTitle")]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResulFeed<ArticleMatrix>>> GetCategoryAndOrTitle(
                                       [FromQuery] int pi = 1, int ps = 10, string? q=null, string? t=null) 
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
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("X-Pagination-Result", $"Mostrando da pagina [{artigos.PageIndex}] ate a pagina [{artigos.PageSize}], num Total de [{artigos.TotalPages}] paginas e [{artigos.TotalResults}] Registros do Banco de Dados.");



            return Ok(artigos);
        }



    }
}
