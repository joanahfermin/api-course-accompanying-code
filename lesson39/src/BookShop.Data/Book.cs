using System.ComponentModel.DataAnnotations;

namespace BookShop.Data;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Author { get; set; } = "";

    [Range(0.01, 10000)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [Required]
    public string Category { get; set; } = "";
}
