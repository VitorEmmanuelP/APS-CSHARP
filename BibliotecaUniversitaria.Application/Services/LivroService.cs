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
            // Verifica se o livro existe sem carregá-lo no contexto
            if (!await _unitOfWork.ExistsLivroAsync(id))
                return false;

            // Busca empréstimos via SQL sem rastreamento (evita problemas de ChangeTracker)
            var emprestimosInfo = await _unitOfWork.QueryEmprestimosByLivroIdAsync(id);

            // Verifica se há empréstimos ativos ou atrasados (não pode deletar)
            var emprestimosAtivosOuAtrasados = emprestimosInfo
                .Where(e => e.Status == (int)Domain.Enums.StatusEmprestimo.Ativo ||
                           e.Status == (int)Domain.Enums.StatusEmprestimo.Atrasado);

            if (emprestimosAtivosOuAtrasados.Any())
                throw new BusinessRuleValidationException("Não é possível excluir um livro com empréstimos ativos ou atrasados");

            // Remove empréstimos devolvidos ou cancelados antes de deletar o livro
            var emprestimosFinalizados = emprestimosInfo
                .Where(e => e.Status == (int)Domain.Enums.StatusEmprestimo.Devolvido ||
                           e.Status == (int)Domain.Enums.StatusEmprestimo.Cancelado)
                .ToList();

            // Usa transação para garantir integridade
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Remove multas e empréstimos finalizados via SQL direto (evita rastreamento do EF Core)
                if (emprestimosFinalizados.Any())
                {
                    var emprestimosIds = emprestimosFinalizados.Select(e => e.Id).ToList();

                    // Remove multas uma por uma para evitar SQL injection
                    foreach (var emprestimoId in emprestimosIds)
                    {
                        await _unitOfWork.ExecuteSqlRawAsync(
                            "DELETE FROM Multas WHERE EmprestimoId = {0}", emprestimoId);
                    }

                    // Remove empréstimos um por um para evitar SQL injection
                    foreach (var emprestimoId in emprestimosIds)
                    {
                        await _unitOfWork.ExecuteSqlRawAsync(
                            "DELETE FROM Emprestimos WHERE Id = {0}", emprestimoId);
                    }
                }

                // Remove o livro via SQL direto (nunca carregado no contexto, evita problemas de navegação)
                await _unitOfWork.ExecuteSqlRawAsync(
                    "DELETE FROM Livros WHERE Id = {0}", id);

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
