using AutoMapper;
using Cooperchip.FeedRSSAnalytics.CoreShare.Configurations;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.UoW;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private static readonly object _lockObj = new();

        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        private readonly AppSettings _appSettings;
        private readonly IArticleMatrixFactory _articleMatrixFactory;
        private readonly IArticleMatrixRepository _articleMatrixRepository;
        private readonly IFeedProcessor _feedProcessor;
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsController(IConfiguration configuration,
                                   IQueryRepository queryRepository,
                                   IMapper mapper,
                                   IOptions<AppSettings> appSettings,
                                   IArticleMatrixFactory articleMatrixFactory,
                                   IArticleMatrixRepository articleMatrixRepository,
                                   IFeedProcessor feedProcessor,
                                   IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _queryRepository = queryRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _articleMatrixFactory = articleMatrixFactory;
            _articleMatrixRepository = articleMatrixRepository;
            _feedProcessor = feedProcessor;
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Route("CreatePosts/{authorId}")]
        public async Task<bool> CreatePosts(string authorId)
        {
            try
            {
                XDocument doc = XDocument.Load($"{_appSettings.BaseUrl}/members/{authorId}/rss");
                if (doc == null)
                {
                    return false;
                }

                var entries = await _feedProcessor.ProcessorFeed(doc, _appSettings);

                var filterByYear = DateTime.Now.Year - 4;

                List<Feed> feeds = entries.OrderByDescending(o => o.PubDate).Where(o => o.PubDate.Year > filterByYear).ToList();

                string urlAddress = string.Empty;
                
                List<ArticleMatrix> articleMatrices = new();

                _ = int.TryParse(_configuration["ParallelTasksCount"], out int parallelTasksCount);

                Console.WriteLine($"\n\n\nNúmero máximo de Threads: {parallelTasksCount.ToString()}");


                Stopwatch cronometro = Stopwatch.StartNew();
                cronometro.Start();

                Parallel.ForEach(feeds, new ParallelOptions { MaxDegreeOfParallelism = parallelTasksCount }, async feed => 
                {
                    urlAddress = feed.Link;

                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(urlAddress)
                    };

                    var result = httpClient.GetAsync("").Result;
                    string strData = "";

                    if (result.StatusCode == HttpStatusCode.OK)
                    {

                        strData = result.Content.ReadAsStringAsync().Result;

                        HtmlDocument htmlDocument = new();
                        htmlDocument.LoadHtml(strData);

                        var articleMatrix = await _articleMatrixFactory.CreateArticleMatrix(authorId, feed, htmlDocument);

                        lock (_lockObj)
                        {
                            articleMatrices.Add(articleMatrix);
                        }
                    }
                });

                await _articleMatrixRepository.RemoveByAuthorIdAsync(authorId);
                await _articleMatrixRepository.AddArticlematrixAsync(articleMatrices);

                // Salvar todas as transações dentro deste mesmo contexto/lifecicle
                _ = await _unitOfWork.Commit();


                cronometro.Stop();
                Console.WriteLine("\n\n\nTempo de decorrido: " + cronometro.ElapsedMilliseconds + " Milissegundos!");


                return true;
            } catch 
            {
                return false;
            }
        }


        [HttpGet]
        [Route("GetCategory/{authorId}")]
        [ProducesResponseType(typeof(List<CategoryDto>), 200)][Produces("application/json")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategory(string authorId)
        {
            if(string.IsNullOrWhiteSpace(authorId)) return BadRequest(string.Empty);

            var categories = _mapper.Map<List<CategoryDto>>(await _queryRepository.GetCategoriesByAuthorId(authorId));

            return Ok(categories);
        }

        [HttpGet]
        [Route("GetAuthors")]
        [ProducesResponseType(typeof(IQueryable<Authors>), 200)][Produces("application/json")]
        public IQueryable<Authors>? GetAuthors()
        {
            return _queryRepository.GetAuthors();
        }

    }
}