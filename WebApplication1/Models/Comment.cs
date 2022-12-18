using WebApplication1.Entities;

namespace Blog.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public User Owner { get; set; }
}