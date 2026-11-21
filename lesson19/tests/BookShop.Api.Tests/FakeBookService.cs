using BookShop.Data;

namespace BookShop.Api.Tests;

public class FakeBookService : IBookService
{
    public List<Book> Books { get; } = new List<Book>();

    public Task<List<Book>> GetAllAsync()
    {
        return Task.FromResult(Books);
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }
}
