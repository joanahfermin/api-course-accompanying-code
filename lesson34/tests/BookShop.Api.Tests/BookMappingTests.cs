using BookShop.Data;

namespace BookShop.Api.Tests;

[TestClass]
public class BookMappingTests
{
    [TestMethod]
    public void ToResponse_Book_CopiesEveryField()
    {
        var book = new Book { Id = 7, Title = "Clean Code", Author = "Robert C. Martin", Price = 34.50m, Stock = 7, Category = "Programming" };

        var response = book.ToResponse();

        Assert.AreEqual(new BookResponse(7, "Clean Code", "Robert C. Martin", 34.50m, 7, "Programming"), response);
    }

    [TestMethod]
    public void ToBook_Request_CopiesTheFieldsAndLeavesTheIdUnset()
    {
        var request = new BookRequest("Clean Code", "Robert C. Martin", 34.50m, 7, "Programming");

        var book = request.ToBook();

        Assert.AreEqual(0, book.Id);
        Assert.AreEqual("Clean Code", book.Title);
        Assert.AreEqual("Robert C. Martin", book.Author);
        Assert.AreEqual(34.50m, book.Price);
        Assert.AreEqual(7, book.Stock);
        Assert.AreEqual("Programming", book.Category);
    }
}
