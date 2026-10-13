namespace DigitalLibrary.API.DTOs.Dashboard;

public class IssuedBookSummaryResponse
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int StudentId { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string BookName { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateOnly IssuedOn { get; set; }
}
