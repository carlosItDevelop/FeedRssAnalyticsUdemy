using AutoMapper;
using Cooperchip.FeedRSSAnalytics.CoreShare.Configurations;
using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.Factory;
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

        public AnalyticsController(IConfiguration configuration,
                                   IQueryRepository queryRepository,
                                   IMapper mapper,
                                   IOptions<AppSettings> appSettings,
                                   IArticleMatrixFactory articleMatrixFactory,
                                   IArticleMatrixRepository articleMatrixRepository,
                                   IFeedProcessor feedProcessor)
        {
            _configuration = configuration;
            _queryRepository = queryRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _articleMatrixFactory = articleMatrixFactory;
            _articleMatrixRepository = articleMatrixRepository;
            _feedProcessor = feedProcessor;
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


                #region
                // Vamos analisar a linha de código abaixo:
                /*
                    Esta linha de código cria uma lista chamada feeds que contém objetos do tipo Feed previamente criados e armazenados na variável entries.

                    Aqui está o que acontece em cada etapa dessa linha de código:

                    entries.OrderByDescending(o => o.PubDate): Ordena os objetos Feed em ordem decrescente de data de publicação (PubDate). Isso garante que os itens mais recentes apareçam primeiro na lista.

                    .Where(d => d.PubDate.Year > 2018): Filtra os objetos Feed para incluir apenas aqueles cuja data de publicação (PubDate) é posterior a 2018. Ou seja, somente itens publicados depois de 2018 serão incluídos na lista feeds.
                
                .ToList(): Converte a sequência resultante dos passos anteriores em uma List<Feed>. Isso cria uma lista concreta de objetos Feed que podem ser usados posteriormente no código.

                    Portanto, a linha de código cria uma lista feeds que contém objetos Feed ordenados em ordem decrescente de data de publicação e filtrados para incluir apenas aqueles publicados após 2018. Essa lista será usada posteriormente no método CreatePosts para realizar outras operações, como extrair informações adicionais das páginas vinculadas aos itens do feed RSS.
                 */
                #endregion

                var filterByYear = DateTime.Now.Year - 4;

                List<Feed> feeds = entries.OrderByDescending(o => o.PubDate).Where(o => o.PubDate.Year > filterByYear).ToList();

                #region
                /*
                 Vejamos a explicação sobre este trecho da mesma action "CreatePosts" na controller "AnalyticsController".:

                string urlAddress = string.Empty;: Essa linha inicializa uma variável chamada urlAddress com uma string vazia. Essa variável será usada posteriormente para armazenar o endereço URL de cada item do feed RSS para acessar e analisar as informações da página.

                List<ArticleMatrix> articleMatrices = new();: Essa linha cria uma nova lista chamada articleMatrices que armazenará objetos do tipo ArticleMatrix. Cada objeto ArticleMatrix conterá informações detalhadas sobre um artigo, como autor, tipo, link, título, data de publicação, categoria, visualizações e curtidas.

                _ = int.TryParse(_configuration["ParallelTasksCount"], out int parallelTasksCount);: Essa linha tenta converter o valor armazenado na chave ParallelTasksCount do objeto de configuração configuration em um número inteiro e armazená-lo na variável parallelTasksCount. Se a conversão for bem-sucedida, parallelTasksCount conterá o número máximo de tarefas que podem ser executadas em paralelo durante o processamento dos itens do feed. O descartável é usado para ignorar o valor de retorno do método TryParse, já que estamos interessados apenas no valor convertido armazenado na variável parallelTasksCount.

                Neste trecho de código, as variáveis urlAddress, articleMatrices e parallelTasksCount são inicializadas para serem usadas posteriormente no método CreatePosts. A variável urlAddress armazenará o endereço URL de cada item do feed, articleMatrices armazenará informações detalhadas sobre cada artigo e parallelTasksCount determinará o número máximo de tarefas que podem ser executadas em paralelo durante o processamento dos itens do feed. Isso facilita a raspagem e a análise eficientes das informações das páginas vinculadas aos itens do feed RSS.
                 */
                #endregion

                string urlAddress = string.Empty;
                
                List<ArticleMatrix> articleMatrices = new();

                _ = int.TryParse(_configuration["ParallelTasksCount"], out int parallelTasksCount);

                Console.WriteLine($"\n\n\nNúmero máximo de Threads: {parallelTasksCount.ToString()}");


                Stopwatch cronometro = Stopwatch.StartNew();
                cronometro.Start();

                Parallel.ForEach(feeds, new ParallelOptions { MaxDegreeOfParallelism = parallelTasksCount }, async feed => 
                {

                    #region: Nota sobre '.Result' & 'await'
                    /*
                    Ao utilizar "var result = httpClient.GetAsync("").Result;", você está bloqueando a execução do código até que a chamada assíncrona seja concluída.Isso é chamado de "sincronização forçada" e pode levar a problemas de desempenho e bloqueio de threads, especialmente em cenários de alto tráfego e tempo de resposta prolongado.

                    Por outro lado, ao utilizar "var result = await httpClient.GetAsync("");", você está usando a palavra - chave "await", que permite que o código continue executando outras tarefas enquanto aguarda a resposta da chamada assíncrona. Isso evita bloqueios desnecessários e melhora o desempenho da aplicação, pois a thread principal não fica parada à espera da resposta da chamada assíncrona.

                    Embora os resultados dos links fornecidos não falem diretamente sobre essa diferença específica, eles ilustram o uso de chamadas assíncronas em C#. No link 1, é possível ver o uso da palavra-chave "await" em conjunto com o método "PostAsync".

                    Em resumo, é recomendável utilizar a abordagem com "await" em chamadas assíncronas para melhorar o desempenho da aplicação e evitar problemas de bloqueio de threads.
                    */
                    #endregion

                    urlAddress = feed.Link;

                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(urlAddress)
                    };

                    var result = await httpClient.GetAsync("");

                    string strData = "";


                    if (result.StatusCode == HttpStatusCode.OK)
                    {

                        strData = await result.Content.ReadAsStringAsync();

                        HtmlDocument htmlDocument = new();

                        htmlDocument.LoadHtml(strData);

                        #region ToDo: Fabrica de Matrix de Artigo
                        var articleMatrix = await _articleMatrixFactory.CreateArticleMatrix(authorId, feed);
                        articleMatrix.Category = ResultCategories.GenerateCategory(htmlDocument);
                        var view = ResultViews.GenerateViews(htmlDocument, articleMatrix);

                        #endregion

                        #region: Old Implementation
                        //string category = "Videos";
                        //if (htmlDocument.GetElementbyId("ImgCategory") != null)
                        //{
                        //    category = htmlDocument.GetElementbyId("ImgCategory").GetAttributeValue("title", "");
                        //}



                        #region: slice notation "[0..^1]"
                        /*
                        O trecho "[0..^1]" presente no código em questão é um slice notation, que indica uma fatia de uma string em C#. Esse slice seleciona os caracteres que vão do primeiro elemento da string (índice 0) até o penúltimo elemento da string (índice -1, representado pelo símbolo ^1). Ou seja, ele seleciona toda a string, exceto o último caractere.

                        Isso é utilizado para remover o 'm' ou o 'k' que podem estar presentes na string "articleMatrix.Views", permitindo que o restante da string seja convertido para um número decimal e multiplicado pelo fator correspondente a cada letra(1000000 para 'm' e 1000 para 'k') para obter o número total de visualizações do artigo.
                        */

                        #endregion


                        //var view = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='ViewCounts']");
                        //if(view != null)
                        //{
                        //    articleMatrix.Views = view.InnerText;

                        //    if (articleMatrix.Views.Contains('m'))
                        //    {
                        //        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
                        //    }
                        //    else if (articleMatrix.Views.Contains('k'))
                        //    {
                        //        articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
                        //    }
                        //    else
                        //    {
                        //        _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
                        //        articleMatrix.ViewsCount = viewCount;
                        //    }
                        //}
                        //else
                        //{
                        //    var newsView = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='spanNewsViews']");
                        //    if (newsView != null)
                        //    {
                        //        articleMatrix.Views = newsView.InnerText;

                        //        if (articleMatrix.Views.Contains('m'))
                        //        {
                        //            articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000000;
                        //        }
                        //        else if (articleMatrix.Views.Contains('k'))
                        //        {
                        //            articleMatrix.ViewsCount = decimal.Parse(articleMatrix.Views[0..^1]) * 1000;
                        //        }
                        //        else
                        //        {
                        //            _ = decimal.TryParse(articleMatrix.Views, out decimal viewCount);
                        //            articleMatrix.ViewsCount = viewCount;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        articleMatrix.ViewsCount = 0;
                        //    }
                        //}
                        #endregion

                        var like = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='LabelLikeCount']");
                        if(like != null)
                        {
                            _ = int.TryParse(like.InnerText, out int likes);
                            articleMatrix.Likes = likes;

                        }

                        lock(_lockObj)
                        {
                            articleMatrices.Add(articleMatrix);
                        }
                    }
                });

                // ToDo BeginTransaction
                await _articleMatrixRepository.RemoveByAuthorIdAsync(authorId);
                await _articleMatrixRepository.AddArticlematrixAsync(articleMatrices);
                // Commit()

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