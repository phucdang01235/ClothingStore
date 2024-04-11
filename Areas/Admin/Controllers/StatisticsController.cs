using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
