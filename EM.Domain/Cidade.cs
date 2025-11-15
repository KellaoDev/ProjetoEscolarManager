using EM.Domain.Enums;
using EM.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain
{
    public class Cidade : IEntidade
    {
        [Key]
        [Required]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome da Cidade")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Display(Name = "UF")]
        public EnumeradorUF EnumeradorUF { get; set; }

        [Display(Name = "Código IBGE")]
        public int? CodigoIBGE { get; set; } 

        public override bool Equals(object? obj)
        {
            return obj is Cidade model &&
                   Codigo == model.Codigo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Codigo);
        }

        public override string ToString()
        {
            return $"Código: {Codigo}, Descrição: {Descricao}, UF: {EnumeradorUF}, Código IBGE: {CodigoIBGE}";
        }
    }
}
