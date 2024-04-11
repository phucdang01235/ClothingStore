using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.user;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserController(
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
        ) {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            


            return View();
        }

        
    }
}
