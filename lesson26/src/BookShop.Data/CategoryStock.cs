namespace BookShop.Data;

public class CategoryStock
{
    public string Category { get; set; } = "";
    public int Titles { get; set; }
    public int Copies { get; set; }
    public decimal StockValue { get; set; }
}
