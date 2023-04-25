using System.ComponentModel.DataAnnotations;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.DTOs
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome da Categoria")]
        [StringLength(80, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Número de Posts nesta Categoria")]
        public int Count { get; set; }
    }
}
