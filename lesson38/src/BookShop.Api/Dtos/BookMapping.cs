using BookShop.Data;

public static class BookMapping
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(book.Id, book.Title, book.Author, book.Price, book.Stock, book.Category);
    }

    public static Book ToBook(this BookRequest request)
    {
        return new Book
        {
            Title = request.Title,
            Author = request.Author,
            Price = request.Price,
            Stock = request.Stock,
            Category = request.Category
        };
    }
}
