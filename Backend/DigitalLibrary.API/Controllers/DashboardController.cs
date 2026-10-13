using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DashboardController(DigitalLibraryDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardResponse>> Get(CancellationToken cancellationToken)
    {
        var totalBooks = await dbContext.Books.CountAsync(cancellationToken);
        var totalStudents = await dbContext.Students.CountAsync(cancellationToken);
        var currentlyIssuedBooks = await dbContext.BookIssues
            .CountAsync(issue => issue.ReturnedOn == null, cancellationToken);
        var availableCopies = await dbContext.Books
            .Select(book => (int?)book.Quantity)
            .SumAsync(cancellationToken) ?? 0;

        var activeIssues = await dbContext.BookIssues
            .AsNoTracking()
            .Where(issue => issue.ReturnedOn == null)
            .Include(issue => issue.Book)
            .Include(issue => issue.Student)
            .OrderByDescending(issue => issue.IssuedOn)
            .ThenByDescending(issue => issue.Id)
            .ToListAsync(cancellationToken);

        var response = new DashboardResponse
        {
            TotalBooks = totalBooks,
            TotalStudents = totalStudents,
            CurrentlyIssuedBooks = currentlyIssuedBooks,
            AvailableCopies = availableCopies,
            IssuedBooks = activeIssues.Select(issue => new IssuedBookSummaryResponse
            {
                Id = issue.Id,
                BookId = issue.BookId,
                StudentId = issue.StudentId,
                ISBN = issue.Book.ISBN,
                BookName = issue.Book.Name,
                Author = issue.Book.Author,
                RollNo = issue.Student.RollNo,
                StudentName = issue.Student.Name,
                IssuedOn = issue.IssuedOn
            }).ToList()
        };

        return Ok(response);
    }
}
