using System.ComponentModel.DataAnnotations;

public record BookRequest(
    [Required] string Title,
    [Required] string Author,
    [Range(0.01, 10000)] decimal Price,
    [Range(0, 100000)] int Stock,
    [Required] string Category);
