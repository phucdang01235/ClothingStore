using ClothingStore.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;

        public LoginController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        
        public async Task<IActionResult> RedirectToView()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRole = _userManager.GetRolesAsync(user).Result;
            if (userRole[0].Equals("Admin"))
            {
                return Redirect("/Admin/Home/Index");
            }
            
            return RedirectToAction("Index", "Home");
        }

    }
}
