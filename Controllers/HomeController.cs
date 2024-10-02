using Microsoft.AspNetCore.Mvc;

namespace newEmpty.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public IActionResult Index()
        {
            return View();
        }

    }
}
