using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs.Reports;
using DigitalLibrary.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsController(DigitalLibraryDbContext dbContext) : ControllerBase
{
    [HttpGet("book-issues")]
    [ProducesResponseType(typeof(IEnumerable<BookIssueReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookIssueReportResponse>>> GetBookIssues(
        [FromQuery] int? bookId,
        [FromQuery] int? studentId,
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate,
        [FromQuery] bool? activeOnly,
        CancellationToken cancellationToken)
    {
        if (bookId is <= 0)
        {
            return BadRequest(new { message = "bookId must be a positive integer." });
        }

        if (studentId is <= 0)
        {
            return BadRequest(new { message = "studentId must be a positive integer." });
        }

        if (fromDate.HasValue && toDate.HasValue && fromDate.Value > toDate.Value)
        {
            return BadRequest(new { message = "fromDate cannot be later than toDate." });
        }

        IQueryable<BookIssue> query = dbContext.BookIssues.AsNoTracking();

        if (bookId.HasValue)
        {
            query = query.Where(issue => issue.BookId == bookId.Value);
        }

        if (studentId.HasValue)
        {
            query = query.Where(issue => issue.StudentId == studentId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(issue => issue.IssuedOn >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(issue => issue.IssuedOn <= toDate.Value);
        }

        if (activeOnly == true)
        {
            query = query.Where(issue => issue.ReturnedOn == null);
        }

        var issues = await query
            .Include(issue => issue.Book)
            .Include(issue => issue.Student)
            .OrderByDescending(issue => issue.IssuedOn)
            .ThenByDescending(issue => issue.Id)
            .ToListAsync(cancellationToken);

        var response = issues.Select(issue => new BookIssueReportResponse
        {
            Id = issue.Id,
            BookId = issue.BookId,
            StudentId = issue.StudentId,
            ISBN = issue.Book.ISBN,
            BookName = issue.Book.Name,
            Author = issue.Book.Author,
            RollNo = issue.Student.RollNo,
            StudentName = issue.Student.Name,
            StudentBranch = issue.Student.Branch,
            IssuedOn = issue.IssuedOn,
            ReturnedOn = issue.ReturnedOn
        });

        return Ok(response);
    }
}
