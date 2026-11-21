using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class BookService : IBookService
{
    private readonly BookShopDbContext _context;

    public BookService(BookShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.OrderBy(b => b.Title).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> UpdateAsync(int id, Book book)
    {
        var existing = await _context.Books.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Price = book.Price;
        existing.Stock = book.Stock;
        existing.Category = book.Category;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Books.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        _context.Books.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SellResult> SellAsync(int id, int quantity)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return new SellResult();
        }

        if (book.Stock < quantity)
        {
            return new SellResult { Book = book };
        }

        book.Stock -= quantity;
        await _context.SaveChangesAsync();
        return new SellResult { Book = book, Sold = true };
    }
}
