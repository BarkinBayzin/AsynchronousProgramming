using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Models.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly IBaseRepository<Category> _categoryRepository;

        public CategoryViewComponent(IBaseRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _categoryRepository.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive));
        }
    }
}
