﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bloogs.Models;
using Bloogs.Data;
using Bloogs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Bloogs.Views.Blog;
using System.Reflection.Metadata;

namespace Bloogs.Controllers
{
    [Authorize(Roles = "Admin, User, Supervisor")]

    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;


        public PostController(AppDbContext context,UserManager<User> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
              return View(await _context.Post.ToListAsync());
        }

        // GET: Post/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }


            var post = await _context.Post.Include(post1 => post1.Poster)
                .Include(post1 => post1.blog)
                .Include(post1 => post1.Comments).ThenInclude(c => c.Owner)
                .Include(post1 => post1.Likers).FirstOrDefaultAsync(post1 => post1.Id == id );
            if (post == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null && !post.blog.IsPublic)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Title")] Post post)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.Owner.Id == user.Id);
            post.Poster = user;
            post.DateCreated = DateTime.Now;
            if (post == null && post.Content.Equals(("")))
            {
                return View(post);
            }

            if (blog.Posts == null)
            {
                blog.Posts = new List<Post>(); 
            }
            blog.Posts.Add(post);
            _context.Add(post);
            _context.Update(blog);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("UserBlog", "Blog", new {@id=post.blog.Id});
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(post1 => post1.Poster)
                .Include(post1 => post1.blog)
                .Include(post1 => post1.Comments).ThenInclude(c => c.Owner)
                .Include(post1 => post1.Likers).FirstOrDefaultAsync(post1 => post1.Id == id );
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (post != null && !post.Content.Equals(("")))
            {
                try
                {
                    var newPost = await _context.Post.Include(post1 => post1.Poster)
                        .Include(post1 => post1.blog)
                        .Include(post1 => post1.Comments).ThenInclude(c => c.Owner)
                        .Include(post1 => post1.Likers).FirstOrDefaultAsync(post1 => post1.Id == id );
                    newPost.Content = post.Content;
                    newPost.Title = post.Title;
                    _context.Update(newPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("UserBlog", "Blog", new {@id=post.blog.Id});
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(p => p.Poster)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        
        // POST: Post/Like/5
        public async Task<IActionResult> Like(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.Post.Include(post1 => post1.Poster)
                    .Include(post1 => post1.blog)
                    .Include(post1 => post1.Comments).ThenInclude(c => c.Owner)
                    .Include(post1 => post1.Likers).FirstOrDefaultAsync(post1 => post1.Id == id );
            if (post == null)
            {
                return NotFound();
            }
            post.Likers?.Add(user);
            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Post", new {@id=post.Id});
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'AppDbContext.Post'  is null.");
            }
            //C'est la descente aux enfers !
            var post = await _context.Post
                .Include(p => p.blog)
                .Include(po=> po.Poster)
                .Include(c => c.Comments).FirstOrDefaultAsync(pos => pos.Id == id);
            
            var blog = post.blog;
            blog.Posts.Remove(post);
            if (post != null)
            {
                //_context.Comment.RemoveRange(post.Comments);
                _context.Post.Remove(post);
                _context.Blog.Update(blog);
            }
            //Vous avez atteint le fond !
            await _context.SaveChangesAsync();
            return RedirectToAction("UserBlog", "Blog", new {@id=blog.Id});
        }

        private bool PostExists(int id)
        {
          return _context.Post.Any(e => e.Id == id);
        }
    }
}
