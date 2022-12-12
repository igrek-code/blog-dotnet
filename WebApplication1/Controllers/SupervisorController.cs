using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace Blog.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SupervisorController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (User user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(User user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
