using System.ComponentModel.DataAnnotations;

public record BookRequest(
    [Required, StringLength(200)] string Title,
    [Required, StringLength(100)] string Author,
    [Range(0.01, 10000), MaxDecimalPlaces(2)] decimal Price,
    [Range(0, 100000)] int Stock,
    [Required, StringLength(50)] string Category);
