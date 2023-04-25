﻿using Cooperchip.FeedRSSAnalytics.Domain.Entities;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services;
using Cooperchip.FeedRssBlogsAnalyticsApi.DTOs;
using Microsoft.AspNetCore.Authorization;
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
            var metadata = new ArtigosMetadataDto(artigos);
            GenericResponseHeader(metadata);


            return Ok(artigos);
        }


        [HttpGet]
        [Route("GetCategoryAndOrTitle")]
        [ProducesResponseType(typeof(PagedResulFeed<ArticleMatrix>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResulFeed<ArticleMatrix>>> GetCategoryAndOrTitle(
                                       [FromQuery] int pi = 1, int ps = 10, string? q=null, string? t=null) 
        {
            var artigos = await _articleMatrixRepository.GetCategoryAndOrTitle(pi, ps, q, t);
            var metadata = new ArtigosMetadataDto(artigos);
            GenericResponseHeader(metadata);

            return Ok(artigos);
        }

        [HttpGet]
        [Route("GetFilterByYear")]
        [ProducesResponseType(typeof(PagedResulFeed<ArticleMatrix>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResulFeed<ArticleMatrix>>> GetFilterByYear(
                    [FromQuery] int pi = 1, int ps = 10, int? q = null)
        {
            var artigos = await _articleMatrixRepository.GetFilterByYear(pi, ps, q);
            var metadata = new ArtigosMetadataDto(artigos);
            GenericResponseHeader(metadata);

            return Ok(artigos);

        }


        private void GenericResponseHeader(ArtigosMetadataDto metadata)
        {
            Response.Headers.Add("Content-Type", "application/json");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("X-Pagination-Result", $"Mostrando da pagina [{metadata.PageIndex}] ate a pagina [{metadata.PageSize}], num Total de [{metadata.TotalPages}] paginas e [{metadata.TotalResults}] Registros do Banco de Dados.");
        }

    }
}