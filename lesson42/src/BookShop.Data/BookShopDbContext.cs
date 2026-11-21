using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class BookShopDbContext : DbContext
{
    public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<AppUser> Users { get; set; }
}
