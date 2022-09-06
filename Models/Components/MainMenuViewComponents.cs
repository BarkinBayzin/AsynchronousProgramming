using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Models.Components
{
    public class MainMenuViewComponents : ViewComponent
    {
        private readonly IBaseRepository<Page> _pageReposity;

        public MainMenuViewComponents(IBaseRepository<Page> pageReposity)
        {
            _pageReposity = pageReposity;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _pageReposity.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive));
        }
    }
}
