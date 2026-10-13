using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs.BookIssues;

public class IssueBookRequest
{
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    [Range(1, int.MaxValue)]
    public int StudentId { get; set; }
}
