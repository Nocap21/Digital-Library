namespace DigitalLibrary.API.Models;

public class Book
{
    public int Id { get; set; }

    public string ISBN { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Publication { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Branch { get; set; } = string.Empty;

    public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
}
