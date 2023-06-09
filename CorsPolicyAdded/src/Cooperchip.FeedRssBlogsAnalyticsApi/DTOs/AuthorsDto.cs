﻿using System.ComponentModel.DataAnnotations;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.DTOs
{
    public class AuthorsDto
    {
        [Display(Name = "Id do Autor")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 1)]
        [Required(ErrorMessage = "O campo {0} é requirido.")]
        public string? AuthorId { get; set; }

        [Display(Name = "Nome do Autor")]
        [StringLength(150, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 1)]
        [Required(ErrorMessage = "O campo {0} é requirido.")]
        public string? Author { get; set; }

        [Display(Name = "Nª de Post do Autor")]
        public int Count { get; set; }
    }
}
