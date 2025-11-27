using Mapster;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.ViewModels;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Exceptions;

namespace BibliotecaUniversitaria.Application.Services
{
    public class LivroService : ILivroService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LivroService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LivroListViewModel>> ObterTodosAsync()
        {
            var livros = await _unitOfWork.Livros.GetAllAsync();
            return livros.Adapt<IEnumerable<LivroListViewModel>>();
        }

        public async Task<LivroViewModel?> ObterPorIdAsync(int id)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(id);
            return livro != null ? livro.Adapt<LivroViewModel>() : null;
        }



        public async Task<IEnumerable<LivroListViewModel>> BuscarPorTituloAsync(string titulo)
        {
            var livros = await _unitOfWork.Livros.GetByTituloAsync(titulo);
            return livros.Adapt<IEnumerable<LivroListViewModel>>();
        }

        public async Task<IEnumerable<LivroListViewModel>> ObterDisponiveisAsync()
        {
            var livros = await _unitOfWork.Livros.GetDisponiveisAsync();
            return livros.Adapt<IEnumerable<LivroListViewModel>>();
        }

        public async Task<LivroViewModel> CriarAsync(LivroViewModel model)
        {
            var autor = await _unitOfWork.Autores.GetByIdAsync(model.AutorId);
            if (autor == null)
                throw new BusinessRuleValidationException("Autor não encontrado");

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(model.CategoriaId);
            if (categoria == null)
                throw new BusinessRuleValidationException("Categoria não encontrada");

            var livro = new Livro(
                model.Titulo,
                model.AutorId,
                model.CategoriaId,
                model.QuantidadeTotal,
                model.Sinopse,
                model.AnoPublicacao,
                model.Editora,
                model.NumeroPaginas
            );

            await _unitOfWork.Livros.AddAsync(livro);
            await _unitOfWork.SaveChangesAsync();

            return livro.Adapt<LivroViewModel>();
        }

        public async Task<LivroViewModel> AtualizarAsync(LivroViewModel model)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(model.Id);
            if (livro == null)
                throw new BusinessRuleValidationException("Livro não encontrado");

            var autor = await _unitOfWork.Autores.GetByIdAsync(model.AutorId);
            if (autor == null)
                throw new BusinessRuleValidationException("Autor não encontrado");

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(model.CategoriaId);
            if (categoria == null)
                throw new BusinessRuleValidationException("Categoria não encontrada");

            livro.SetTitulo(model.Titulo);
            livro.SetAutorId(model.AutorId);
            livro.SetCategoriaId(model.CategoriaId);
            livro.SetQuantidadeTotal(model.QuantidadeTotal);
            livro.SetSinopse(model.Sinopse);
            livro.SetAnoPublicacao(model.AnoPublicacao);
            livro.SetEditora(model.Editora);
            livro.SetNumeroPaginas(model.NumeroPaginas);

            _unitOfWork.Livros.Update(livro);
            await _unitOfWork.SaveChangesAsync();

            return livro.Adapt<LivroViewModel>();
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(id);
            if (livro == null)
                return false;

            var todosEmprestimos = await _unitOfWork.Emprestimos.GetByLivroIdAsync(id);

            // Verifica se há empréstimos ativos ou atrasados (não pode deletar)
            var emprestimosAtivosOuAtrasados = todosEmprestimos
                .Where(e => e.Status == Domain.Enums.StatusEmprestimo.Ativo ||
                           e.Status == Domain.Enums.StatusEmprestimo.Atrasado);

            if (emprestimosAtivosOuAtrasados.Any())
                throw new BusinessRuleValidationException("Não é possível excluir um livro com empréstimos ativos ou atrasados");

            // Remove empréstimos devolvidos ou cancelados antes de deletar o livro
            var emprestimosFinalizados = todosEmprestimos
                .Where(e => e.Status == Domain.Enums.StatusEmprestimo.Devolvido ||
                           e.Status == Domain.Enums.StatusEmprestimo.Cancelado)
                .ToList();

            // Usa transação para garantir integridade
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                bool temMultasParaRemover = false;

                // Remove multas relacionadas aos empréstimos finalizados
                foreach (var emprestimo in emprestimosFinalizados)
                {
                    var multas = await _unitOfWork.Multas.GetByEmprestimoIdAsync(emprestimo.Id);
                    if (multas.Any())
                    {
                        _unitOfWork.Multas.RemoveRange(multas);
                        temMultasParaRemover = true;
                    }
                }

                // Salva as multas removidas primeiro (se houver)
                if (temMultasParaRemover)
                {
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove os empréstimos finalizados
                if (emprestimosFinalizados.Any())
                {
                    _unitOfWork.Emprestimos.RemoveRange(emprestimosFinalizados);
                    // Salva a remoção dos empréstimos antes de remover o livro
                    await _unitOfWork.SaveChangesAsync();
                }

                // Remove o livro (agora sem empréstimos relacionados)
                _unitOfWork.Livros.Remove(livro);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _unitOfWork.Livros.ExistsAsync(l => l.Id == id);
        }
    }
}
