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

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        //[Cpf]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        //[DataNascimento]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        public EnumeradorSexo EnumeradorSexo { get; set; }

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
