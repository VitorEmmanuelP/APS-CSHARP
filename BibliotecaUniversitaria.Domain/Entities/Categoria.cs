using System;
using System.Collections.Generic;

namespace BibliotecaUniversitaria.Domain.Entities
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        private readonly List<Livro> _livros = new List<Livro>();
        public IReadOnlyCollection<Livro> Livros => _livros.AsReadOnly();

        private Categoria() { }

        public Categoria(string nome, string descricao = null)
        {
            SetNome(nome);
            SetDescricao(descricao);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da categoria não pode ser vazio");

            Nome = nome.Trim();
            UpdateTimestamp();
        }

        public void SetDescricao(string descricao)
        {
            Descricao = descricao?.Trim();
            UpdateTimestamp();
        }
    }
}
