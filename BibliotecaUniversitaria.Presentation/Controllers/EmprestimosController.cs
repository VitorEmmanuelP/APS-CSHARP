using Microsoft.AspNetCore.Mvc;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.DTOs;
using BibliotecaUniversitaria.Application.ViewModels;

namespace BibliotecaUniversitaria.Presentation.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly IEmprestimoService _emprestimoService;
        private readonly ILivroService _livroService;

        public EmprestimosController(IEmprestimoService emprestimoService, ILivroService livroService)
        {
            _emprestimoService = emprestimoService;
            _livroService = livroService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var emprestimos = await _emprestimoService.ObterTodosAsync();
            return View(emprestimos);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _emprestimoService.ObterPorIdAsync(id.Value);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CarregarDadosParaView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmprestimoCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emprestimoService.CriarAsync(model);
                    TempData["SuccessMessage"] = "Empréstimo criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            await CarregarDadosParaView();
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Devolver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _emprestimoService.ObterPorIdAsync(id.Value);
            if (emprestimo == null)
            {
                return NotFound();
            }

            var devolucaoDto = new EmprestimoDevolucaoDTO
            {
                DataDevolucaoReal = DateTime.Now
            };

            ViewBag.Emprestimo = emprestimo;
            return View(devolucaoDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id, EmprestimoDevolucaoDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emprestimoService.DevolverAsync(id, model);
                    TempData["SuccessMessage"] = "Livro devolvido com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            var emprestimo = await _emprestimoService.ObterPorIdAsync(id);
            ViewBag.Emprestimo = emprestimo;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Cancelar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _emprestimoService.ObterPorIdAsync(id.Value);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost, ActionName("Cancelar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmado(int id)
        {
            try
            {
                var sucesso = await _emprestimoService.CancelarAsync(id);
                if (sucesso)
                {
                    TempData["SuccessMessage"] = "Empréstimo cancelado com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erro ao cancelar empréstimo.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Ativos()
        {
            var emprestimos = await _emprestimoService.ObterAtivosAsync();
            return View("Index", emprestimos);
        }


        [HttpGet]
        public async Task<IActionResult> Atrasados()
        {
            var emprestimos = await _emprestimoService.ObterAtrasadosAsync();
            return View("Index", emprestimos);
        }

        private async Task CarregarDadosParaView()
        {
            var livros = await _livroService.ObterDisponiveisAsync();

            ViewBag.Livros = livros.Select(l => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Titulo
            }).ToList();
        }
    }
}
