using EM.Domain.Enums;
using EM.Domain.Interfaces;
using EM.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain
{
    public  class Aluno : IEntidade
    {
        [Key]
        [Required]
        [Display(Name = "Matrícula")]
        public int Matricula {  get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome Completo")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        public string Nome { get; set; }

        [Cpf]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [DataNascimento]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Sexo")]
        public EnumeradorSexo EnumeradorSexo { get; set; }

        [Display(Name = "Cidade")]
        public int CidadeId { get; set; }

        public Cidade? Cidade { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Aluno model &&
                   Matricula == model.Matricula;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Matricula);
        }

        public override string ToString()
        {
            return $"Matrícula: {Matricula}";
        }
    }
}
