using AutoMapper;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.ViewModels;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Exceptions;

namespace BibliotecaUniversitaria.Application.Services
{
    public class LivroService : ILivroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LivroService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LivroListViewModel>> ObterTodosAsync()
        {
            var livros = await _unitOfWork.Livros.GetAllAsync();
            return _mapper.Map<IEnumerable<LivroListViewModel>>(livros);
        }

        public async Task<LivroViewModel?> ObterPorIdAsync(int id)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(id);
            return livro != null ? _mapper.Map<LivroViewModel>(livro) : null;
        }



        public async Task<IEnumerable<LivroListViewModel>> BuscarPorTituloAsync(string titulo)
        {
            var livros = await _unitOfWork.Livros.GetByTituloAsync(titulo);
            return _mapper.Map<IEnumerable<LivroListViewModel>>(livros);
        }

        public async Task<IEnumerable<LivroListViewModel>> ObterDisponiveisAsync()
        {
            var livros = await _unitOfWork.Livros.GetDisponiveisAsync();
            return _mapper.Map<IEnumerable<LivroListViewModel>>(livros);
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

            return _mapper.Map<LivroViewModel>(livro);
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

            return _mapper.Map<LivroViewModel>(livro);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var livro = await _unitOfWork.Livros.GetByIdAsync(id);
            if (livro == null)
                return false;

            var emprestimosAtivos = await _unitOfWork.Emprestimos.GetByLivroIdAsync(id);
            if (emprestimosAtivos.Any(e => e.Status == Domain.Enums.StatusEmprestimo.Ativo))
                throw new BusinessRuleValidationException("Não é possível excluir um livro com empréstimos ativos");

            _unitOfWork.Livros.Remove(livro);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _unitOfWork.Livros.ExistsAsync(l => l.Id == id);
        }
    }
}
