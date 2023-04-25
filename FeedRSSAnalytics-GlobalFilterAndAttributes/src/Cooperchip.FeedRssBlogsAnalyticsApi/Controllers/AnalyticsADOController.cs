using AutoMapper;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsADOController : ControllerBase
    {
        private readonly IQueryADORepository _queryADORepository;
        private readonly IMapper _mapper;

        public AnalyticsADOController(IQueryADORepository queryADORepository, 
                                      IMapper mapper)
        {
            _queryADORepository = queryADORepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Resumo analítico de Categoria
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns>Retorna uma lista de Categoria por Id</returns>
        /// <remarks>
        /// Exemplo de Request:
        /// 
        ///      GET /Todo
        ///      {
        ///         "name": "Nome da categoria",
        ///         "count": Qtde de Por na Categoria"
        ///      }
        /// </remarks>
        [HttpGet]
        [Route("GetCategory/{authorId}")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategory(string authorId)
        {
            if(string.IsNullOrWhiteSpace(authorId)) return BadRequest(string.Empty);

            var author = await _queryADORepository.GetCategoriesByAuthorId(authorId);
            if (author == null) return NotFound();

            var categories = _mapper.Map<IEnumerable<CategoryDto>>(author);

            return Ok(categories);
        }


        /// <summary>
        /// Resumo por Agrupamento de Post por Autor
        /// </summary>
        /// <returns>Resumo por Agrupamento de Post por Autor</returns>
        /// <remarks>
        /// Exemplo de Request:
        /// 
        ///      GET /Todo
        ///      {
        ///         "authorId": "Id do Author",
        ///         "author": "Nome do Author",
        ///         "count": Qtde de Post deste Author"
        ///      }
        /// </remarks>
        [HttpGet]
        [Route("GetAuthors")]
        [ProducesResponseType(typeof(IEnumerable<AuthorsDto>), 200)]
        public async Task<ActionResult<IEnumerable<AuthorsDto>>> GetAuthors()
        {
            var iEnumAuthor = await _queryADORepository.GetAuthors();

            return Ok(_mapper.Map<IEnumerable<AuthorsDto>>(iEnumAuthor));
        }


        /// <summary>
        /// Retorna uma lista de Feeds (Artigo, Blog, video, etc.) do Author.
        /// </summary>
        /// <returns>Retorna uma lista de Feeds (Artigo, Blog, video, etc.) do Author.</returns>
        /// <remarks>
        /// Retorna uma lista de Feeds (Artigo, Blog, video, etc.) do Author.
        /// Exemplo de Request:
        /// 
        ///      GET /Todo
        ///      {
        ///         "id": "Id do Author",
        ///         "authorId": "Apelido do Author",
        ///         "author": "Nome do Author",
        ///         "link": "Link do Artigo",
        ///         "title": "Título do Artigo",
        ///         "type": "Tipo do Artigo",
        ///         "category": "Categoria do Artigo",
        ///         "views": "1.5k",
        ///         "viewsCount": 15000, [exemplo...]
        ///         "likes": 2, [exemplo...]
        ///         "pubDate": "2023-02-18T00:00:00" [exemplo...]
        ///      }
        /// </remarks>
        [HttpGet]
        [Route("GetAll/{authorId}")]
        [ProducesResponseType(typeof(IEnumerable<ArticleMatrixDto>), 200)]
        public async Task<ActionResult<IEnumerable<ArticleMatrixDto>>> GetAll(string authorId)
        {
            var model = await _queryADORepository.GetAllArticlesByAuthorId(authorId);
            var aMapper = _mapper.Map<IEnumerable<ArticleMatrixDto>>(model);
            return Ok(aMapper);
        }

    }
}
