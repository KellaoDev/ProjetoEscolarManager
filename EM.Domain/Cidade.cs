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

        [Required]
        [Display(Name = "Nome da Cidade")]
        public string Descricao { get; set; }

        [Required]
        public string UF { get; set; }

        [Display(Name = "Código IBGE")]
        public int CodigoIBGE { get; set; } 

        public override bool Equals(object? obj)
        {
            return obj is Cidade model &&
                   Codigo == model.Codigo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Codigo);
        }
    }
}
