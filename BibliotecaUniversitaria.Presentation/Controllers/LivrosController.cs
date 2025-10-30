using Microsoft.AspNetCore.Mvc;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.ViewModels;
using BibliotecaUniversitaria.Application.DTOs;
using AutoMapper;

namespace BibliotecaUniversitaria.Presentation.Controllers
{
    public class LivrosController : Controller
    {
        private readonly ILivroService _livroService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LivrosController(ILivroService livroService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _livroService = livroService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var livros = await _livroService.ObterTodosAsync();
            return View(livros);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.ObterPorIdAsync(id.Value);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LivroViewModel();
            await CarregarDadosParaView(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _livroService.CriarAsync(model);
                    TempData["SuccessMessage"] = "Livro criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await CarregarDadosParaView(model);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.ObterPorIdAsync(id.Value);
            if (livro == null)
            {
                return NotFound();
            }

            await CarregarDadosParaView(livro);
            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LivroViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _livroService.AtualizarAsync(model);
                    TempData["SuccessMessage"] = "Livro atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await CarregarDadosParaView(model);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.ObterPorIdAsync(id.Value);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _livroService.ExcluirAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Livro excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Livro não encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Search(string? titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                var todosLivros = await _livroService.ObterTodosAsync();
                return View("Index", todosLivros);
            }

            var livros = await _livroService.BuscarPorTituloAsync(titulo);
            return View("Index", livros);
        }


        [HttpGet]
        public async Task<IActionResult> Disponiveis()
        {
            var livros = await _livroService.ObterDisponiveisAsync();
            return View("Index", livros);
        }

        private async Task CarregarDadosParaView(LivroViewModel model)
        {
            var autores = await _unitOfWork.Autores.GetAllAsync();
            var categorias = await _unitOfWork.Categorias.GetAllAsync();

            ViewBag.Autores = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(autores, "Id", "Nome");
            ViewBag.Categorias = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categorias, "Id", "Nome");
        }
    }
}
