using System;

namespace BibliotecaUniversitaria.Domain.Exceptions
{
    public class BusinessRuleValidationException : DomainException
    {
        public BusinessRuleValidationException(string message) : base(message)
        {
        }

        public BusinessRuleValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
