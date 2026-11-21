using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Tests;

[TestClass]
public class BookValidationTests
{
    private static Book ValidBook()
    {
        return new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Price = 34.50m,
            Stock = 7,
            Category = "Programming"
        };
    }

    private static List<ValidationResult> Validate(Book book)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(book, new ValidationContext(book), results, validateAllProperties: true);
        return results;
    }

    [TestMethod]
    public void Validate_ValidBook_HasNoErrors()
    {
        var results = Validate(ValidBook());

        Assert.IsEmpty(results);
    }

    [TestMethod]
    public void Validate_EmptyTitle_HasAnErrorOnTitle()
    {
        var book = ValidBook();
        book.Title = "";

        var results = Validate(book);

        Assert.HasCount(1, results);
        Assert.Contains("Title", results[0].MemberNames);
    }

    [TestMethod]
    public void Validate_PriceZero_HasAnErrorOnPrice()
    {
        var book = ValidBook();
        book.Price = 0m;

        var results = Validate(book);

        Assert.HasCount(1, results);
        Assert.Contains("Price", results[0].MemberNames);
    }

    [TestMethod]
    public void Validate_NegativeStock_HasAnErrorOnStock()
    {
        var book = ValidBook();
        book.Stock = -1;

        var results = Validate(book);

        Assert.HasCount(1, results);
        Assert.Contains("Stock", results[0].MemberNames);
    }
}
