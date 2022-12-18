using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext (DbContextOptions<BlogContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Blog.Models.Blog> Blog { get; set; } = default!;
    }
}
