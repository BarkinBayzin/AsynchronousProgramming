using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Models.Components
{
    public class MainMenuViewComponents : ViewComponent
    {
        private readonly IPageRepository _pageReposity;

        public MainMenuViewComponents(IPageRepository pageReposity)
        {
            _pageReposity = pageReposity;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var a = await _pageReposity.GetByDefaults(x => x.Status != Entities.Abstract.Status.Passive);
            return View(a);
        }
    }
}
