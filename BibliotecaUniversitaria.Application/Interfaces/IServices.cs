using BibliotecaUniversitaria.Application.DTOs;
using BibliotecaUniversitaria.Application.ViewModels;

namespace BibliotecaUniversitaria.Application.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<LivroListViewModel>> ObterTodosAsync();
        Task<LivroViewModel?> ObterPorIdAsync(int id);
        Task<IEnumerable<LivroListViewModel>> BuscarPorTituloAsync(string titulo);
        Task<IEnumerable<LivroListViewModel>> ObterDisponiveisAsync();
        Task<LivroViewModel> CriarAsync(LivroViewModel model);
        Task<LivroViewModel> AtualizarAsync(LivroViewModel model);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }


    public interface IEmprestimoService
    {
        Task<IEnumerable<EmprestimoDTO>> ObterTodosAsync();
        Task<EmprestimoDTO?> ObterPorIdAsync(int id);
        Task<IEnumerable<EmprestimoDTO>> ObterAtivosAsync();
        Task<IEnumerable<EmprestimoDTO>> ObterAtrasadosAsync();
        Task<EmprestimoDTO> CriarAsync(EmprestimoCreateDTO dto);
        Task<EmprestimoDTO> DevolverAsync(int id, EmprestimoDevolucaoDTO dto);
        Task<bool> CancelarAsync(int id);
        Task<bool> ExisteAsync(int id);
    }
}
