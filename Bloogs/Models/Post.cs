using System.ComponentModel.DataAnnotations;
using Bloogs.Entities;

namespace Bloogs.Models;

public class Post
{
    public int Id { get; set; }
    [DataType(DataType.MultilineText)]
    public string Content { get; set; }
    public virtual Blog blog { get; set; }

    public string Title { get; set; }
    public User Poster { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<User>? Likers { get; set; }

}