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

    private static BookRequest RequestFor(string title)
    {
        return new BookRequest(title, "Robert C. Martin", 34.50m, 7, "Programming");
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
        var result = await _controller.Create(RequestFor("Clean Code"));

        var created = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(created);
        Assert.AreEqual("GetById", created.ActionName);
        Assert.AreEqual(1, created.RouteValues!["id"]);
        var book = created.Value as BookResponse;
        Assert.IsNotNull(book);
        Assert.AreEqual("Clean Code", book.Title);
    }

    [TestMethod]
    public async Task Create_ValidBook_PassesItToTheService()
    {
        await _controller.Create(RequestFor("Clean Code"));

        Assert.HasCount(1, _fakeService.Books);
        Assert.AreEqual("Clean Code", _fakeService.Books[0].Title);
    }

    [TestMethod]
    public async Task Update_ExistingBook_ReturnsNoContent()
    {
        _fakeService.Books.Add(new Book { Id = 3, Title = "Clean Code" });

        var result = await _controller.Update(3, RequestFor("Clean Code, 2nd edition"));

        Assert.IsInstanceOfType<NoContentResult>(result);
    }

    [TestMethod]
    public async Task Update_ExistingBook_PassesTheNewValuesToTheService()
    {
        _fakeService.Books.Add(new Book { Id = 3, Title = "Clean Code" });

        await _controller.Update(3, RequestFor("Clean Code, 2nd edition"));

        Assert.AreEqual("Clean Code, 2nd edition", _fakeService.Books[0].Title);
    }

    [TestMethod]
    public async Task Update_UnknownId_ReturnsNotFound()
    {
        var result = await _controller.Update(999, RequestFor("Clean Code"));

        Assert.IsInstanceOfType<NotFoundResult>(result);
    }

    [TestMethod]
    public async Task Delete_ExistingBook_ReturnsNoContentAndRemovesIt()
    {
        _fakeService.Books.Add(new Book { Id = 3, Title = "Clean Code" });

        var result = await _controller.Delete(3);

        Assert.IsInstanceOfType<NoContentResult>(result);
        Assert.IsEmpty(_fakeService.Books);
    }

    [TestMethod]
    public async Task Delete_UnknownId_ReturnsNotFound()
    {
        var result = await _controller.Delete(999);

        Assert.IsInstanceOfType<NotFoundResult>(result);
    }

    [TestMethod]
    public async Task Sell_EnoughCopies_ReturnsNoContent()
    {
        _fakeService.SellResult = new SellResult { Book = new Book { Title = "Refactoring", Stock = 1 }, Sold = true };

        var result = await _controller.Sell(3, 2);

        Assert.IsInstanceOfType<NoContentResult>(result);
    }

    [TestMethod]
    public async Task Sell_NotEnoughCopies_ReturnsConflictWithAMeaningfulProblem()
    {
        _fakeService.SellResult = new SellResult { Book = new Book { Title = "Refactoring", Stock = 3 } };

        var result = await _controller.Sell(3, 5);

        var answer = result as ObjectResult;
        Assert.IsNotNull(answer);
        Assert.AreEqual(409, answer.StatusCode);
        var problem = answer.Value as ProblemDetails;
        Assert.IsNotNull(problem);
        Assert.AreEqual("https://bookshop.example/errors/insufficient-stock", problem.Type);
        Assert.Contains("Refactoring", problem.Detail!);
        Assert.Contains("3", problem.Detail!);
        Assert.Contains("5", problem.Detail!);
    }

    [TestMethod]
    public async Task Sell_UnknownBook_ReturnsNotFound()
    {
        _fakeService.SellResult = new SellResult();

        var result = await _controller.Sell(999, 1);

        Assert.IsInstanceOfType<NotFoundResult>(result);
    }
}
