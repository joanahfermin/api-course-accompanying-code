using BookShop.Data;

namespace BookShop.Api.Tests;

public class FakeBookService : IBookService
{
    public List<Book> Books { get; } = [];
    public SellResult SellResult { get; set; } = new();

    public Task<List<Book>> GetAllAsync()
    {
        return Task.FromResult(Books);
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<Book> CreateAsync(Book book)
    {
        book.Id = Books.Count + 1;
        Books.Add(book);
        return Task.FromResult(book);
    }

    public Task<bool> UpdateAsync(int id, Book book)
    {
        var index = Books.FindIndex(b => b.Id == id);
        if (index < 0)
        {
            return Task.FromResult(false);
        }

        book.Id = id;
        Books[index] = book;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var existing = Books.FirstOrDefault(b => b.Id == id);
        if (existing == null)
        {
            return Task.FromResult(false);
        }

        Books.Remove(existing);
        return Task.FromResult(true);
    }

    public Task<SellResult> SellAsync(int id, int quantity)
    {
        return Task.FromResult(SellResult);
    }
}
