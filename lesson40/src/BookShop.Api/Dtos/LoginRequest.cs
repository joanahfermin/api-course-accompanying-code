using System.ComponentModel.DataAnnotations;

public record LoginRequest([Required] string UserName, [Required] string Password);
