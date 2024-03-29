﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Bloogs.Models;
using Bloogs.Data;
using Bloogs.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger,
            AppDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
           

                var blogs = _context.Blog
                    .Include(b => b.Posts).ThenInclude(p => p.Poster)
                    .Include(b => b.Posts).ThenInclude(p => p.Comments)
                    .Include(b => b.Posts).ThenInclude(p => p.Likers).ToList();
                if (!_signInManager.IsSignedIn(User))
                {
                    blogs = blogs.FindAll(b => b.IsPublic);
                }

                var posts = blogs.SelectMany(b => b.Posts != null ? b.Posts : new List<Post>()).ToList();

            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}