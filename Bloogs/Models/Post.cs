using System.ComponentModel.DataAnnotations;
using WebApplication1.Entities;

namespace Bloogs.Models;

public class Post
{
    public int Id { get; set; }
    [DataType(DataType.MultilineText)]
    public string Content { get; set; }
    public string ImagesUrl { get; set; }
    public User Poster { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<User>? Likers { get; set; }

}