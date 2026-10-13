namespace DigitalLibrary.API.Models;

public class Student
{
    public int Id { get; set; }

    public string RollNo { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
}
