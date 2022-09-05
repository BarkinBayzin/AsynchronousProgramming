using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.DTOs;
using AsynchronousProgramming.Models.Entities.Concrete;
using AsynchronousProgramming.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IBaseRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IActionResult Create() => View();
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var slug = await _categoryRepository.GetByDefault(x => x.Slug == model.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError(string.Empty, "The category already exist...!");
                    TempData["Warning"] = "The category already exist...!";
                    return View();
                }
                else
                {
                    var category = _mapper.Map<Category>(model);
                    await _categoryRepository.Add(category);
                    TempData["Success"] = "The category has been created...!";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["Error"] = "The category hasn't been created!";
                return View(model);
            }
        }
        public IActionResult List()
        {
            var list = _categoryRepository.Where(x => x.Status != Models.Entities.Abstract.Status.Passive);
            CategoryVM vm = new CategoryVM();
            vm.Categories.AddRange(list);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _categoryRepository.GetById(id);
            var model = _mapper.Map<UpdateCategoryDTO>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var slug = await _categoryRepository.GetByDefault(x => x.Slug == model.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError(string.Empty, $"{model.Name} already exist...!");
                    TempData["Warning"] = "The category already exist...!";
                    return View();
                }
                else
                {
                    var category = _mapper.Map<Category>(model);
                    await _categoryRepository.Update(category);
                    TempData["Success"] = "The category has been updated...!";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["Error"] = "The category hasn't been updated..!";
                return View(model);
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            Category category = await _categoryRepository.GetById(id);

            if (category != null)
            {
                await _categoryRepository.Delete(category);
                TempData["Success"] = "The category has been removed..!";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Warning"] = "There is no such a category..!";
                return RedirectToAction("List");
            }
        }
    }
}
