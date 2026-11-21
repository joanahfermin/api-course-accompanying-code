using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data.Tests;

[TestClass]
public class UserServiceTests
{
    private SqliteConnection _connection = null!;
    private BookShopDbContext _context = null!;
    private UserService _service = null!;

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

        _context.Users.Add(new AppUser
        {
            UserName = "alice",
            Role = "Manager",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret")
        });
        _context.SaveChanges();

        _service = new UserService(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [TestMethod]
    public async Task FindUserAsync_RightPassword_ReturnsTheUserWithTheirRole()
    {
        var user = await _service.FindUserAsync("alice", "secret");

        Assert.IsNotNull(user);
        Assert.AreEqual("Manager", user.Role);
    }

    [TestMethod]
    public async Task FindUserAsync_WrongPassword_ReturnsNull()
    {
        var user = await _service.FindUserAsync("alice", "guess");

        Assert.IsNull(user);
    }
}
