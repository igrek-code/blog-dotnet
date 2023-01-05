using Bloogs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bloogs.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Reflection.Metadata;
using Bloogs.Data;

namespace Bloogs.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public SupervisorController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserRolesManage()
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

        public async Task<IActionResult> RoleManage()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("RoleManage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (roleId != null)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(roleId);
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("RoleManage");
        }

        public async Task<IActionResult> SpecificUserRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("Index");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SpecificUserRole(List<ManageUserRolesViewModel> model, string userId, string name, bool isPublic)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View(model);
            }
            var userRoleList = await _userManager.GetRolesAsync(user);
            var roleToAdd = new List<String>();
            var roleToRemove = new List<String>();
            foreach (var role in model)
            {
                if (role.Selected && !userRoleList.Contains(role.RoleName))
                {
                    roleToAdd.Add(role.RoleName);
                }
                else if(!role.Selected && userRoleList.Contains(role.RoleName))
                {
                    roleToRemove.Add(role.RoleName);
                }
            }
            if (roleToAdd.Contains("Admin"))
            {
                if (name != null && !name.Equals(""))
                {
                    var blog = new Blog();
                    blog.Owner = user;
                    blog.Name = name;
                    if (blog.Owner != null && blog.Name != null && !blog.Name.Equals(""))
                    {
                        _context.Add(blog);
                        await _context.SaveChangesAsync();
                    }

                }
            }
            if (roleToRemove.Contains("Admin"))
            {
                var blog = await _context.Blog
                    .FirstOrDefaultAsync(m => m.Owner.Id == userId);
                if (blog != null)
                {
                    _context.Blog.Remove(blog);
                }

                await _context.SaveChangesAsync();
            }
            await _userManager.RemoveFromRolesAsync(user, roleToRemove);
            await _userManager.AddToRolesAsync(user, roleToAdd);

            return RedirectToAction("UserRolesManage");
        }
    }
}
