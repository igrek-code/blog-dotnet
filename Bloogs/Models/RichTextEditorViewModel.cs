using System.ComponentModel.DataAnnotations;

namespace Bloogs.Models;

public class RichTextEditorViewModel
{
     [Display(Name = "Message")] public string Message { get; set; }
}