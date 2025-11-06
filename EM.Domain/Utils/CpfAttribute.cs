using EM.Domain.Utils.Validations;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Utils
{
    public class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string Cpf || string.IsNullOrWhiteSpace(Cpf))
            {
                return new ValidationResult("CPF é obrigatório");
            }

            if (!Validacoes.ValidaCpf(Cpf))
            {
                return new ValidationResult("CPF é inválido");
            }

            return ValidationResult.Success;
        }   
    }
}
