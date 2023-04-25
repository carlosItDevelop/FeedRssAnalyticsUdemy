using AutoMapper;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {

        readonly CultureInfo culture = new("en-US");
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private static readonly object _lockObj = new();

        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public AnalyticsController(ApplicationDbContext dbContext, IConfiguration configuration, IQueryRepository queryRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetCategory/{authorId}")]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryDto>>> GetCategory(string authorId)
        {
            if(string.IsNullOrWhiteSpace(authorId)) return BadRequest(string.Empty);

            var categories = _mapper.Map<List<CategoryDto>>(await _queryRepository.GetCategoriesByAuthorId(authorId));

            return Ok(categories);
        }

        [HttpGet]
        [Route("GetAuthors")]
        public IQueryable<Authors>? GetAuthors()
        {
            return _queryRepository.GetAuthors();
        }

    }
}
