using System;

namespace BibliotecaUniversitaria.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; }
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
