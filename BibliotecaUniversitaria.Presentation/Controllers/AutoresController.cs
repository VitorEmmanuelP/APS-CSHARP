using Microsoft.AspNetCore.Mvc;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.DTOs;
using Mapster;

namespace BibliotecaUniversitaria.Presentation.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var autores = await _unitOfWork.Autores.GetAllAsync();
            var autoresDto = autores.Adapt<IEnumerable<AutorDTO>>();
            return View(autoresDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _unitOfWork.Autores.GetByIdAsync(id.Value);
            if (autor == null)
            {
                return NotFound();
            }

            var autorDto = autor.Adapt<AutorDTO>();
            return View(autorDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutorCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var autor = new Domain.Entities.Autor(
                        dto.Nome,
                        dto.Biografia,
                        dto.DataNascimento,
                        dto.Nacionalidade
                    );

                    await _unitOfWork.Autores.AddAsync(autor);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Autor criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _unitOfWork.Autores.GetByIdAsync(id.Value);
            if (autor == null)
            {
                return NotFound();
            }

            var autorDto = autor.Adapt<AutorCreateDTO>();
            return View(autorDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AutorCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var autor = await _unitOfWork.Autores.GetByIdAsync(id);
                    if (autor == null)
                    {
                        return NotFound();
                    }

                    autor.SetNome(dto.Nome);
                    autor.SetBiografia(dto.Biografia);
                    autor.SetDataNascimento(dto.DataNascimento);
                    autor.SetNacionalidade(dto.Nacionalidade);

                    _unitOfWork.Autores.Update(autor);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Autor atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _unitOfWork.Autores.GetByIdAsync(id.Value);
            if (autor == null)
            {
                return NotFound();
            }

            var autorDto = autor.Adapt<AutorDTO>();
            return View(autorDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var autor = await _unitOfWork.Autores.GetByIdAsync(id);
                if (autor == null)
                {
                    TempData["ErrorMessage"] = "Autor não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var livros = await _unitOfWork.Livros.GetByAutorIdAsync(id);
                if (livros.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir um autor que possui livros associados.";
                    return RedirectToAction(nameof(Index));
                }

                _unitOfWork.Autores.Remove(autor);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Autor excluído com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
