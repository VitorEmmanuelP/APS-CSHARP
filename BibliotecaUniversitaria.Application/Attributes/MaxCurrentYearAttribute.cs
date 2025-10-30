using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUniversitaria.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MaxCurrentYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is int year)
            {
                var currentYear = DateTime.Now.Year;
                if (year <= currentYear && year >= 0)
                {
                    return ValidationResult.Success;
                }

                var error = string.IsNullOrWhiteSpace(ErrorMessage)
                    ? $"Ano de publicação deve ser entre 0 e {currentYear}."
                    : ErrorMessage;
                return new ValidationResult(error);
            }

            return new ValidationResult("Valor de ano inválido.");
        }
    }
}


