﻿using Bloogs.Entities;

namespace Bloogs.Models;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public User Owner { get; set; }
    
    public DateTime DateCreated { get; set; }
    
    public Boolean IsPublic { get; set; }
    public virtual List<Post>? Posts { get; set; }

}