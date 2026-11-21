using BookShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Tests;

[TestClass]
public class BooksControllerTests
{
    private FakeBookService _fakeService = null!;
    private BooksController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _fakeService = new FakeBookService();
        _controller = new BooksController(_fakeService);
    }

    [TestMethod]
    public async Task GetAll_TwoBooks_ReturnsBoth()
    {
        _fakeService.Books.Add(new Book { Id = 1, Title = "Clean Code" });
        _fakeService.Books.Add(new Book { Id = 2, Title = "Refactoring" });

        var result = await _controller.GetAll();

        Assert.IsNotNull(result.Value);
        Assert.HasCount(2, result.Value);
    }

    [TestMethod]
    public async Task GetById_ExistingBook_ReturnsIt()
    {
        _fakeService.Books.Add(new Book { Id = 7, Title = "Clean Code" });

        var result = await _controller.GetById(7);

        Assert.IsNotNull(result.Value);
        Assert.AreEqual("Clean Code", result.Value.Title);
    }

    [TestMethod]
    public async Task GetById_UnknownId_ReturnsNotFound()
    {
        var result = await _controller.GetById(999);

        Assert.IsInstanceOfType<NotFoundResult>(result.Result);
    }

    [TestMethod]
    public async Task Create_ValidBook_ReturnsCreatedPointingAtTheNewBook()
    {
        var result = await _controller.Create(new Book { Title = "Clean Code" });

        var created = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(created);
        Assert.AreEqual("GetById", created.ActionName);
        Assert.AreEqual(1, created.RouteValues!["id"]);
        var book = created.Value as Book;
        Assert.IsNotNull(book);
        Assert.AreEqual("Clean Code", book.Title);
    }

    [TestMethod]
    public async Task Create_ValidBook_PassesItToTheService()
    {
        await _controller.Create(new Book { Title = "Clean Code" });

        Assert.HasCount(1, _fakeService.Books);
        Assert.AreEqual("Clean Code", _fakeService.Books[0].Title);
    }
}
