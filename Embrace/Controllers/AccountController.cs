using Microsoft.AspNetCore.Mvc;

namespace Embrace.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
