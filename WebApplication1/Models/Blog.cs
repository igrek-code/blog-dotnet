﻿using WebApplication1.Entities;

namespace Blog.Models;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public User Owner { get; set; }
    public Boolean IsPublic { get; set; }

}