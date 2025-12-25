using ProniaApp.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ProniaApp.Models;

public class Category : BaseEntity
{
    [MaxLength(30, ErrorMessage = "Name 30dan cox yazmaq olmaz")]
    public string Name { get; set; }

    public List<Product>? Products { get; set; }
}
