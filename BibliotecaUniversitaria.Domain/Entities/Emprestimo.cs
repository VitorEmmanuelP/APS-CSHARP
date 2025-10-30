using System;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Domain.Entities
{
    public class Emprestimo : Entity
    {
        public int LivroId { get; private set; }
        public DateTime DataEmprestimo { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }
        public int QuantidadeEmprestada { get; private set; }
        public StatusEmprestimo Status { get; private set; }
        public string Observacoes { get; private set; }

        public Livro Livro { get; private set; }

        private Emprestimo() { }

        public Emprestimo(int livroId, DateTime dataEmprestimo, DateTime dataDevolucaoPrevista, int quantidadeEmprestada, string observacoes = null)
        {
            SetLivroId(livroId);
            SetDataEmprestimo(dataEmprestimo);
            SetDataDevolucaoPrevista(dataDevolucaoPrevista);
            SetQuantidadeEmprestada(quantidadeEmprestada);
            SetObservacoes(observacoes);
            Status = StatusEmprestimo.Ativo;
        }

        public void SetLivroId(int livroId)
        {
            if (livroId <= 0)
                throw new ArgumentException("ID do livro deve ser maior que zero");

            LivroId = livroId;
            UpdateTimestamp();
        }

        public void SetQuantidadeEmprestada(int quantidadeEmprestada)
        {
            if (quantidadeEmprestada <= 0)
                throw new ArgumentException("Quantidade emprestada deve ser maior que zero");

            QuantidadeEmprestada = quantidadeEmprestada;
            UpdateTimestamp();
        }

        public void SetDataEmprestimo(DateTime dataEmprestimo)
        {
            if (dataEmprestimo > DateTime.Now)
                throw new ArgumentException("Data de empréstimo não pode ser futura");

            DataEmprestimo = dataEmprestimo;
            UpdateTimestamp();
        }

        public void SetDataDevolucaoPrevista(DateTime dataDevolucaoPrevista)
        {
            if (dataDevolucaoPrevista <= DataEmprestimo)
                throw new ArgumentException("Data de devolução prevista deve ser posterior à data de empréstimo");

            DataDevolucaoPrevista = dataDevolucaoPrevista;
            UpdateTimestamp();
        }

        public void SetObservacoes(string observacoes)
        {
            Observacoes = observacoes?.Trim();
            UpdateTimestamp();
        }

        public void Devolver(DateTime dataDevolucaoReal, string observacoes = null)
        {
            if (Status != StatusEmprestimo.Ativo)
                throw new InvalidOperationException("Apenas empréstimos ativos podem ser devolvidos");

            if (dataDevolucaoReal < DataEmprestimo)
                throw new ArgumentException("Data de devolução não pode ser anterior à data de empréstimo");

            DataDevolucaoReal = dataDevolucaoReal;
            if (!string.IsNullOrWhiteSpace(observacoes))
            {
                SetObservacoes(observacoes);
            }
            Status = StatusEmprestimo.Devolvido;
            UpdateTimestamp();
        }

        public void Cancelar()
        {
            if (Status != StatusEmprestimo.Ativo)
                throw new InvalidOperationException("Apenas empréstimos ativos podem ser cancelados");

            Status = StatusEmprestimo.Cancelado;
            UpdateTimestamp();
        }

        public void MarcarComoAtrasado()
        {
            if (Status != StatusEmprestimo.Ativo)
                throw new InvalidOperationException("Apenas empréstimos ativos podem ser marcados como atrasados");

            if (DateTime.Now <= DataDevolucaoPrevista)
                throw new InvalidOperationException("Empréstimo ainda não está atrasado");

            Status = StatusEmprestimo.Atrasado;
            UpdateTimestamp();
        }

        public bool EstaAtrasado()
        {
            return Status == StatusEmprestimo.Ativo && DateTime.Now > DataDevolucaoPrevista;
        }

        public int DiasAtraso()
        {
            if (!EstaAtrasado())
                return 0;

            return (DateTime.Now - DataDevolucaoPrevista).Days;
        }
    }
}
