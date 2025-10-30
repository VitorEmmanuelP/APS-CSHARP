using Microsoft.AspNetCore.Mvc;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.DTOs;
using AutoMapper;

namespace BibliotecaUniversitaria.Presentation.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _unitOfWork.Categorias.GetAllAsync();
            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return View(categoriasDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return View(categoriaDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoria = new Domain.Entities.Categoria(dto.Nome, dto.Descricao);

                    await _unitOfWork.Categorias.AddAsync(categoria);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Categoria criada com sucesso!";
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

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaCreateDTO>(categoria);
            return View(categoriaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaCreateDTO dto)
        {
            if (id != id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
                    if (categoria == null)
                    {
                        return NotFound();
                    }

                    categoria.SetNome(dto.Nome);
                    categoria.SetDescricao(dto.Descricao);

                    _unitOfWork.Categorias.Update(categoria);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Categoria atualizada com sucesso!";
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

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return View(categoriaDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
                if (categoria == null)
                {
                    TempData["ErrorMessage"] = "Categoria não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                var livros = await _unitOfWork.Livros.GetByCategoriaIdAsync(id);
                if (livros.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir uma categoria que possui livros associados.";
                    return RedirectToAction(nameof(Index));
                }

                _unitOfWork.Categorias.Remove(categoria);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Categoria excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
