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
}
