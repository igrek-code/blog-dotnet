using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bloogs.Models;
using Bloogs.Data;
using Bloogs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Syncfusion.Blazor.Data;

namespace Bloogs.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BlogController(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager; 
        }
        
        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blog.Include(b => b.Posts)
                .Include(b => b.Owner).ToListAsync();
            if (!_signInManager.IsSignedIn(User))
            {
                blogs = blogs.FindAll(b => b.IsPublic); 
            }
              return View(blogs);
        }
        
        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null && !blog.IsPublic)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            }

            return View(blog);
        }

        [Authorize(Roles = "Supervisor")]
        // GET: Blog/Create
        public IActionResult Create()
        {
            return View();
        }
        
        
        // GET: Blog/UserBlog/8
        public async Task<IActionResult> UserBlog(int id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.Include(b1 => b1.Owner)
                .Include(b => b.Posts)
                .ThenInclude(p => p.Comments).Include(b2=> b2.Posts).ThenInclude(l=>l.Likers)
                .FirstOrDefaultAsync(bl => bl.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            // blog.Posts = await _context.Post.Where(p => p.blog.Id == blog.Id).ToListAsync(); 
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null && !blog.IsPublic)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            return View(blog);
        }

        [Authorize(Roles = "Supervisor")]
        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsPublic")] Blog blog)
        {
            blog.Owner = await _userManager.GetUserAsync(HttpContext.User);
            blog.Posts = new List<Post>();
            if (blog.Owner != null && blog.IsPublic != null && blog.Name != null && !blog.Name.Equals(""))
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }
        
        [Authorize(Roles = "Admin,Supervisor")]
        // GET: Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || (!roles.Contains("Admin") && !roles.Contains("Supervisor")) )
                return RedirectToAction("Index", "Blog");

            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.Include(b => b.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if(roles.Contains("Supervisor") || user.Id == blog.Owner.Id)
                return View(blog);

 
            return NotFound();
 
        }

        [Authorize(Roles = "Admin")]
        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (blog != null && !blog.Name.Equals(""))
            {
                try
                {
                    var newBlog = await _context.Blog.FindAsync(id);
                    newBlog.Name = blog.Name;
                    newBlog.IsPublic = blog.IsPublic; 
                    _context.Update(newBlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        [Authorize(Roles = "Admin,Supervisor")]
        // GET: Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [Authorize(Roles = "Admin,Supervisor")]
        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blog == null)
            {
                return Problem("Entity set 'AppDbContext.Blog'  is null.");
            }
            var blog = await _context.Blog.FindAsync(id);
            if (blog != null)
            {
                _context.Blog.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return _context.Blog.Any(e => e.Id == id);
        }
    }
}
