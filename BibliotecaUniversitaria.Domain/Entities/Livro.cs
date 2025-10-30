using System;
using System.Collections.Generic;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Domain.Entities
{
    public class Livro : Entity
    {
        public string Titulo { get; private set; }
        public string Sinopse { get; private set; }
        public int AnoPublicacao { get; private set; }
        public int QuantidadeDisponivel { get; private set; }
        public int QuantidadeTotal { get; private set; }
        public string Editora { get; private set; }
        public int NumeroPaginas { get; private set; }

        public int AutorId { get; private set; }
        public int CategoriaId { get; private set; }

        public Autor Autor { get; private set; }
        public Categoria Categoria { get; private set; }
        private readonly List<Emprestimo> _emprestimos = new List<Emprestimo>();
        public IReadOnlyCollection<Emprestimo> Emprestimos => _emprestimos.AsReadOnly();

        private Livro() { }

        public Livro(string titulo, int autorId, int categoriaId, int quantidadeTotal,
                    string sinopse = null, int anoPublicacao = 0, string editora = null, int numeroPaginas = 0)
        {
            SetTitulo(titulo);
            SetAutorId(autorId);
            SetCategoriaId(categoriaId);
            SetQuantidadeTotal(quantidadeTotal);
            SetSinopse(sinopse);
            SetAnoPublicacao(anoPublicacao);
            SetEditora(editora);
            SetNumeroPaginas(numeroPaginas);
            QuantidadeDisponivel = quantidadeTotal;
        }

        public void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título do livro não pode ser vazio");

            Titulo = titulo.Trim();
            UpdateTimestamp();
        }


        public void SetAutorId(int autorId)
        {
            if (autorId <= 0)
                throw new ArgumentException("ID do autor deve ser maior que zero");

            AutorId = autorId;
            UpdateTimestamp();
        }

        public void SetCategoriaId(int categoriaId)
        {
            if (categoriaId <= 0)
                throw new ArgumentException("ID da categoria deve ser maior que zero");

            CategoriaId = categoriaId;
            UpdateTimestamp();
        }

        public void SetQuantidadeTotal(int quantidade)
        {
            if (quantidade < 0)
                throw new ArgumentException("Quantidade total não pode ser negativa");

            QuantidadeTotal = quantidade;
            UpdateTimestamp();
        }

        public void SetSinopse(string sinopse)
        {
            Sinopse = sinopse?.Trim();
            UpdateTimestamp();
        }

        public void SetAnoPublicacao(int ano)
        {
            if (ano < 0 || ano > DateTime.Now.Year)
                throw new ArgumentException("Ano de publicação inválido");

            AnoPublicacao = ano;
            UpdateTimestamp();
        }

        public void SetEditora(string editora)
        {
            Editora = editora?.Trim();
            UpdateTimestamp();
        }

        public void SetNumeroPaginas(int paginas)
        {
            if (paginas < 0)
                throw new ArgumentException("Número de páginas não pode ser negativo");

            NumeroPaginas = paginas;
            UpdateTimestamp();
        }

        public bool EstaDisponivel()
        {
            return QuantidadeDisponivel > 0;
        }

        public void Emprestar()
        {
            if (!EstaDisponivel())
                throw new InvalidOperationException("Livro não está disponível para empréstimo");

            QuantidadeDisponivel--;
            UpdateTimestamp();
        }

        public void Devolver()
        {
            if (QuantidadeDisponivel >= QuantidadeTotal)
                throw new InvalidOperationException("Todas as cópias já foram devolvidas");

            QuantidadeDisponivel++;
            UpdateTimestamp();
        }
    }
}
