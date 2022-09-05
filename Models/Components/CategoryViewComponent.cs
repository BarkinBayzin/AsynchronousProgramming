using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Models.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _categoryRepository.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive));
        }
    }
}
