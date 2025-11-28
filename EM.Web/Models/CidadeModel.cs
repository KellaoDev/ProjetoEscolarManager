using EM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EM.Web.Models
{
    public class CidadeModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome da Cidade")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "UF")]
        public EnumeradorUF EnumeradorUF { get; set; }
    }
}
