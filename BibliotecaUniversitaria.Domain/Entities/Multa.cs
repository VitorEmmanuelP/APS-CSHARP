using System;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Domain.Entities
{
    public class Multa : Entity
    {
        public int EmprestimoId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public StatusMulta Status { get; private set; }
        public string Observacoes { get; private set; }

        public Emprestimo Emprestimo { get; private set; }

        private Multa() { }

        public Multa(int emprestimoId, decimal valor, DateTime dataVencimento)
        {
            SetEmprestimoId(emprestimoId);
            SetValor(valor);
            SetDataVencimento(dataVencimento);
            Status = StatusMulta.Pendente;
        }

        public void SetEmprestimoId(int emprestimoId)
        {
            if (emprestimoId <= 0)
                throw new ArgumentException("ID do empréstimo deve ser maior que zero");

            EmprestimoId = emprestimoId;
            UpdateTimestamp();
        }

        public void SetValor(decimal valor)
        {
            if (valor < 0)
                throw new ArgumentException("Valor da multa não pode ser negativo");

            Valor = valor;
            UpdateTimestamp();
        }

        public void SetDataVencimento(DateTime dataVencimento)
        {
            if (dataVencimento <= DateTime.Now)
                throw new ArgumentException("Data de vencimento deve ser futura");

            DataVencimento = dataVencimento;
            UpdateTimestamp();
        }

        public void SetObservacoes(string observacoes)
        {
            Observacoes = observacoes?.Trim();
            UpdateTimestamp();
        }

        public void Pagar(DateTime dataPagamento)
        {
            if (Status != StatusMulta.Pendente)
                throw new InvalidOperationException("Apenas multas pendentes podem ser pagas");

            if (dataPagamento < CreatedAt)
                throw new ArgumentException("Data de pagamento não pode ser anterior à criação da multa");

            DataPagamento = dataPagamento;
            Status = StatusMulta.Paga;
            UpdateTimestamp();
        }

        public void Cancelar()
        {
            if (Status != StatusMulta.Pendente)
                throw new InvalidOperationException("Apenas multas pendentes podem ser canceladas");

            Status = StatusMulta.Cancelada;
            UpdateTimestamp();
        }

        public bool EstaVencida()
        {
            return Status == StatusMulta.Pendente && DateTime.Now > DataVencimento;
        }

        public int DiasVencida()
        {
            if (!EstaVencida())
                return 0;

            return (DateTime.Now - DataVencimento).Days;
        }
    }
}
