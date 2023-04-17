using System.ComponentModel.DataAnnotations;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.DTOs
{
    public class AuthorsDto
    {
        [Display(Name = "Id do Autor")]
        public string? AuthorId { get; set; }

        [Display(Name = "Nome do Autor")]
        public string? Author { get; set; }

        [Display(Name = "Nª de Post do Autor")]
        public int Count { get; set; }
    }
}
