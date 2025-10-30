using System;
using System.Collections.Generic;

namespace BibliotecaUniversitaria.Domain.Entities
{
    public class Autor : Entity
    {
        public string Nome { get; private set; }
        public string Biografia { get; private set; }
        public DateTime? DataNascimento { get; private set; }
        public string Nacionalidade { get; private set; }

        private readonly List<Livro> _livros = new List<Livro>();
        public IReadOnlyCollection<Livro> Livros => _livros.AsReadOnly();

        private Autor() { }

        public Autor(string nome, string biografia = null, DateTime? dataNascimento = null, string nacionalidade = null)
        {
            SetNome(nome);
            SetBiografia(biografia);
            SetDataNascimento(dataNascimento);
            SetNacionalidade(nacionalidade);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do autor não pode ser vazio");

            Nome = nome.Trim();
            UpdateTimestamp();
        }

        public void SetBiografia(string biografia)
        {
            Biografia = biografia?.Trim();
            UpdateTimestamp();
        }

        public void SetDataNascimento(DateTime? dataNascimento)
        {
            if (dataNascimento.HasValue && dataNascimento.Value > DateTime.Now)
                throw new ArgumentException("Data de nascimento não pode ser futura");

            DataNascimento = dataNascimento;
            UpdateTimestamp();
        }

        public void SetNacionalidade(string nacionalidade)
        {
            Nacionalidade = nacionalidade?.Trim();
            UpdateTimestamp();
        }
    }
}
