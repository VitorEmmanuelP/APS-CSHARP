using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BibliotecaUniversitaria.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NotOnlyWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string strValue)
            {
                if (string.IsNullOrWhiteSpace(strValue))
                {
                    var error = string.IsNullOrWhiteSpace(ErrorMessage)
                        ? "O campo não pode conter apenas espaços em branco."
                        : ErrorMessage;
                    return new ValidationResult(error);
                }

                // Verifica se após remover espaços ainda há conteúdo
                if (strValue.Trim().Length == 0)
                {
                    var error = string.IsNullOrWhiteSpace(ErrorMessage)
                        ? "O campo não pode conter apenas espaços em branco."
                        : ErrorMessage;
                    return new ValidationResult(error);
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}

