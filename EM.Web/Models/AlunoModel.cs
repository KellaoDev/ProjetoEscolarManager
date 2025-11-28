using EM.Domain.Enums;
using EM.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EM.Web.Models
{
    public class AlunoModel
    {
        [Required]
        [Display(Name = "Matrícula")]
        public int Matricula { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome Completo")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Cpf]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [DataNascimento]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Sexo")]
        public EnumeradorSexo EnumeradorSexo { get; set; }

        public CidadeModel? Cidade { get; set; }
    }
}
