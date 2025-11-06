using EM.Domain.Utils.Validations;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Utils
{
    public class DataNascimentoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime DataNascimento)
            {
                return new ValidationResult("Data de Nascimento é obrigatória");
            }

            if (!Validacoes.ValidaDataNascimento(DataNascimento))
            {
                return new ValidationResult("Data de Nascimento é inválida");
            }

            return ValidationResult.Success;
        }
    }
}
