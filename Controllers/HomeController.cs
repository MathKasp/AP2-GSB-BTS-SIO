using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace newEmpty.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

    }
}

// TO FIX

// Add fonctionne mais ne tien pas antécédent + allergies