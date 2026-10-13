namespace DigitalLibrary.API.Models;

public class BookIssue
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int StudentId { get; set; }

    public DateOnly IssuedOn { get; set; }

    public DateOnly? ReturnedOn { get; set; }

    public Book Book { get; set; } = null!;

    public Student Student { get; set; } = null!;
}
