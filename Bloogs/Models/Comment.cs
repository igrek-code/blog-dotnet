﻿using System.ComponentModel.DataAnnotations;
using Bloogs.Entities;

namespace Bloogs.Models;

public class Comment
{
    public int Id { get; set; }
    [DataType(DataType.MultilineText), Required]
    
    public string Content { get; set; }
    public User Owner { get; set; }
}