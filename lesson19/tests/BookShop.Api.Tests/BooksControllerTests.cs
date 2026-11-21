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
}
