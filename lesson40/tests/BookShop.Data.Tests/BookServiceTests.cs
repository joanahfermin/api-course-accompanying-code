using System.ComponentModel.DataAnnotations;
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

    private static Book ValidBook(string title)
    {
        return new Book { Title = title, Author = "Robert C. Martin", Price = 34.50m, Stock = 7, Category = "Programming" };
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
        var book = ValidBook("Clean Code");

        var created = await _service.CreateAsync(book);

        Assert.IsGreaterThan(0, created.Id);
        var found = await _service.GetByIdAsync(created.Id);
        Assert.IsNotNull(found);
        Assert.AreEqual("Clean Code", found.Title);
    }

    [TestMethod]
    public async Task CreateAsync_TwoBooks_GetDifferentIds()
    {
        var first = await _service.CreateAsync(ValidBook("Clean Code"));
        var second = await _service.CreateAsync(ValidBook("Refactoring"));

        Assert.AreNotEqual(first.Id, second.Id);
    }

    [TestMethod]
    public async Task CreateAsync_NegativePrice_IsRefused()
    {
        var book = new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = -1m, Stock = 7, Category = "Programming" };

        await Assert.ThrowsExactlyAsync<ValidationException>(() => _service.CreateAsync(book));
    }

    [TestMethod]
    public async Task UpdateAsync_ExistingBook_ReplacesItsValues()
    {
        var saved = new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m, Stock = 7, Category = "Programming" };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();
        var replacement = new Book { Title = "Clean Code, 2nd edition", Author = "Robert C. Martin", Price = 29.90m, Stock = 20, Category = "Software" };

        var found = await _service.UpdateAsync(saved.Id, replacement);

        Assert.IsTrue(found);
        _context.ChangeTracker.Clear();
        var book = await _service.GetByIdAsync(saved.Id);
        Assert.IsNotNull(book);
        Assert.AreEqual("Clean Code, 2nd edition", book.Title);
        Assert.AreEqual(29.90m, book.Price);
        Assert.AreEqual(20, book.Stock);
        Assert.AreEqual("Software", book.Category);
    }

    [TestMethod]
    public async Task UpdateAsync_BodyHasAnotherId_KeepsTheIdOfTheAddress()
    {
        var saved = new Book { Title = "Clean Code" };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var replacement = ValidBook("Clean Code, 2nd edition");
        replacement.Id = 99;
        await _service.UpdateAsync(saved.Id, replacement);

        _context.ChangeTracker.Clear();
        Assert.IsNull(await _service.GetByIdAsync(99));
        var book = await _service.GetByIdAsync(saved.Id);
        Assert.IsNotNull(book);
        Assert.AreEqual("Clean Code, 2nd edition", book.Title);
    }

    [TestMethod]
    public async Task UpdateAsync_UnknownId_ReturnsFalse()
    {
        var found = await _service.UpdateAsync(999, ValidBook("Clean Code"));

        Assert.IsFalse(found);
    }

    [TestMethod]
    public async Task DeleteAsync_ExistingBook_RemovesIt()
    {
        var saved = new Book { Title = "Clean Code" };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var found = await _service.DeleteAsync(saved.Id);

        Assert.IsTrue(found);
        _context.ChangeTracker.Clear();
        Assert.IsNull(await _service.GetByIdAsync(saved.Id));
    }

    [TestMethod]
    public async Task DeleteAsync_UnknownId_ReturnsFalse()
    {
        var found = await _service.DeleteAsync(999);

        Assert.IsFalse(found);
    }

    [TestMethod]
    public async Task SellAsync_EnoughCopies_ReducesTheStock()
    {
        var saved = new Book { Title = "Refactoring", Stock = 3 };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var result = await _service.SellAsync(saved.Id, 2);

        Assert.IsTrue(result.Sold);
        _context.ChangeTracker.Clear();
        var book = await _service.GetByIdAsync(saved.Id);
        Assert.IsNotNull(book);
        Assert.AreEqual(1, book.Stock);
    }

    [TestMethod]
    public async Task SellAsync_ExactlyTheCopiesLeft_SellsThemAll()
    {
        var saved = new Book { Title = "Refactoring", Stock = 3 };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var result = await _service.SellAsync(saved.Id, 3);

        Assert.IsTrue(result.Sold);
        Assert.IsNotNull(result.Book);
        Assert.AreEqual(0, result.Book.Stock);
    }

    [TestMethod]
    public async Task SellAsync_NotEnoughCopies_SellsNothingAndKeepsTheStock()
    {
        var saved = new Book { Title = "Refactoring", Stock = 3 };
        _context.Books.Add(saved);
        await _context.SaveChangesAsync();

        var result = await _service.SellAsync(saved.Id, 5);

        Assert.IsFalse(result.Sold);
        Assert.IsNotNull(result.Book);
        Assert.AreEqual("Refactoring", result.Book.Title);
        Assert.AreEqual(3, result.Book.Stock);
        _context.ChangeTracker.Clear();
        var book = await _service.GetByIdAsync(saved.Id);
        Assert.IsNotNull(book);
        Assert.AreEqual(3, book.Stock);
    }

    [TestMethod]
    public async Task SellAsync_UnknownId_ReturnsNoBook()
    {
        var result = await _service.SellAsync(999, 1);

        Assert.IsNull(result.Book);
        Assert.IsFalse(result.Sold);
    }
}
