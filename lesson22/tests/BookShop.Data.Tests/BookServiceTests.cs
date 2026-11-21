using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data.Tests;

[TestClass]
public class BookServiceTests
{
    private SqliteConnection _connection = null!;
    private BookShopDbContext _context = null!;
    private BookService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<BookShopDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new BookShopDbContext(options);
        _context.Database.EnsureCreated();

        _service = new BookService(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [TestMethod]
    public async Task GetAllAsync_NoBooks_ReturnsEmptyList()
    {
        var books = await _service.GetAllAsync();

        Assert.IsEmpty(books);
    }

    [TestMethod]
    public async Task GetAllAsync_SomeBooks_ReturnsThemOrderedByTitle()
    {
        _context.Books.Add(new Book { Title = "Refactoring", Author = "Martin Fowler", Price = 44.00m });
        _context.Books.Add(new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m });
        await _context.SaveChangesAsync();

        var books = await _service.GetAllAsync();

        Assert.HasCount(2, books);
        Assert.AreEqual("Clean Code", books[0].Title);
        Assert.AreEqual("Refactoring", books[1].Title);
    }

    [TestMethod]
    public async Task GetByIdAsync_ExistingBook_ReturnsIt()
    {
        var saved = new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var book = await _service.GetByIdAsync(saved.Id);

        Assert.IsNotNull(book);
        Assert.AreEqual("Clean Code", book.Title);
    }

    [TestMethod]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        var book = await _service.GetByIdAsync(999);

        Assert.IsNull(book);
    }

    [TestMethod]
    public async Task CreateAsync_ValidBook_SavesItAndGivesItAnId()
    {
        var book = new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m };

        var created = await _service.CreateAsync(book);

        Assert.IsGreaterThan(0, created.Id);
        var found = await _service.GetByIdAsync(created.Id);
        Assert.IsNotNull(found);
        Assert.AreEqual("Clean Code", found.Title);
    }

    [TestMethod]
    public async Task CreateAsync_TwoBooks_GetDifferentIds()
    {
        var first = await _service.CreateAsync(new Book { Title = "Clean Code" });
        var second = await _service.CreateAsync(new Book { Title = "Refactoring" });

        Assert.AreNotEqual(first.Id, second.Id);
    }
}
