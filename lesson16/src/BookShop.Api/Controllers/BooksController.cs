using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public List<Book> GetAll()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 39.90m },
            new Book { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m },
            new Book { Id = 3, Title = "Refactoring", Author = "Martin Fowler", Price = 44.00m }
        };
        return books;
    }
}
