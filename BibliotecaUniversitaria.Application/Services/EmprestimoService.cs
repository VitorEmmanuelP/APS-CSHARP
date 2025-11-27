using Mapster;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.DTOs;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;
using BibliotecaUniversitaria.Domain.Exceptions;

namespace BibliotecaUniversitaria.Application.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmprestimoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmprestimoDTO>> ObterTodosAsync()
        {
            var emprestimos = await _unitOfWork.Emprestimos.GetAllAsync();
            return emprestimos.Adapt<IEnumerable<EmprestimoDTO>>();
        }

        public async Task<EmprestimoDTO?> ObterPorIdAsync(int id)
        {
            var emprestimo = await _unitOfWork.Emprestimos.GetByIdAsync(id);
            return emprestimo != null ? emprestimo.Adapt<EmprestimoDTO>() : null;
        }


        public async Task<IEnumerable<EmprestimoDTO>> ObterAtivosAsync()
        {
            var emprestimos = await _unitOfWork.Emprestimos.GetAtivosAsync();
            return emprestimos.Adapt<IEnumerable<EmprestimoDTO>>();
        }

        public async Task<IEnumerable<EmprestimoDTO>> ObterAtrasadosAsync()
        {
            var emprestimos = await _unitOfWork.Emprestimos.GetAtrasadosAsync();
            return emprestimos.Adapt<IEnumerable<EmprestimoDTO>>();
        }

        public async Task<EmprestimoDTO> CriarAsync(EmprestimoCreateDTO dto)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(dto.LivroId);
            if (livro == null)
                throw new BusinessRuleValidationException("Livro não encontrado");

            if (dto.QuantidadeEmprestada > livro.QuantidadeDisponivel)
                throw new BusinessRuleValidationException($"Quantidade solicitada ({dto.QuantidadeEmprestada}) é maior que a disponível ({livro.QuantidadeDisponivel})");

            var emprestimo = new Emprestimo(
                dto.LivroId,
                dto.DataEmprestimo,
                dto.DataDevolucaoPrevista,
                dto.QuantidadeEmprestada,
                dto.Observacoes
            );

            for (int i = 0; i < dto.QuantidadeEmprestada; i++)
            {
                livro.Emprestar();
            }

            await _unitOfWork.Emprestimos.AddAsync(emprestimo);
            _unitOfWork.Livros.Update(livro);
            await _unitOfWork.SaveChangesAsync();

            return emprestimo.Adapt<EmprestimoDTO>();
        }

        public async Task<EmprestimoDTO> DevolverAsync(int id, EmprestimoDevolucaoDTO dto)
        {
            var emprestimo = await _unitOfWork.Emprestimos.GetByIdAsync(id);
            if (emprestimo == null)
                throw new BusinessRuleValidationException("Empréstimo não encontrado");

            if (emprestimo.Status != StatusEmprestimo.Ativo)
                throw new BusinessRuleValidationException("Empréstimo não está ativo");

            var livro = await _unitOfWork.Livros.GetByIdAsync(emprestimo.LivroId);
            if (livro != null)
            {
                for (int i = 0; i < emprestimo.QuantidadeEmprestada; i++)
                {
                    livro.Devolver();
                }
                _unitOfWork.Livros.Update(livro);
            }

            emprestimo.Devolver(dto.DataDevolucaoReal, dto.Observacoes);
            _unitOfWork.Emprestimos.Update(emprestimo);
            await _unitOfWork.SaveChangesAsync();

            return emprestimo.Adapt<EmprestimoDTO>();
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var emprestimo = await _unitOfWork.Emprestimos.GetByIdAsync(id);
            if (emprestimo == null)
                return false;

            if (emprestimo.Status != StatusEmprestimo.Ativo)
                throw new BusinessRuleValidationException("Apenas empréstimos ativos podem ser cancelados");

            var livro = await _unitOfWork.Livros.GetByIdAsync(emprestimo.LivroId);
            if (livro != null)
            {
                for (int i = 0; i < emprestimo.QuantidadeEmprestada; i++)
                {
                    livro.Devolver();
                }
                _unitOfWork.Livros.Update(livro);
            }

            emprestimo.Cancelar();
            _unitOfWork.Emprestimos.Update(emprestimo);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _unitOfWork.Emprestimos.ExistsAsync(e => e.Id == id);
        }
    }
}
