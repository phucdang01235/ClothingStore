using ClothingStore.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
        ) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string roleName, string oldRole)
        {
            if(roleName == null)
                return RedirectToAction("Index", "User");

            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == userId);
            await _userManager.RemoveFromRoleAsync(user, oldRole);
            await _userManager.AddToRoleAsync(user, roleName);
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> DisableUSer(string userId, bool isDisable)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // Vô hiệu / Kích hoạt người dùng
            user.IsDisable = !isDisable;
            // Update user vào database
            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "User");
        }
    }
}
