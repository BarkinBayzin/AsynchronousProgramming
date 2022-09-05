using Microsoft.AspNetCore.Mvc;

namespace AsynchronousProgramming.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
