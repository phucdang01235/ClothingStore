using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
