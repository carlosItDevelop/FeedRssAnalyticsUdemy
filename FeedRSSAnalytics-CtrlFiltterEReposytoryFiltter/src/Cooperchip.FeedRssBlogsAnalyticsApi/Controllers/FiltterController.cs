using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Microsoft.AspNetCore.Mvc;

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

    }
}
