using System.Data;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs.BookIssues;
using DigitalLibrary.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookIssuesController(DigitalLibraryDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookIssueResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookIssueResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var issues = await dbContext.BookIssues
            .AsNoTracking()
            .Include(issue => issue.Book)
            .Include(issue => issue.Student)
            .OrderByDescending(issue => issue.IssuedOn)
            .ThenByDescending(issue => issue.Id)
            .ToListAsync(cancellationToken);

        return Ok(issues.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookIssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookIssueResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var issue = await dbContext.BookIssues
            .AsNoTracking()
            .Include(item => item.Book)
            .Include(item => item.Student)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return issue is null
            ? NotFound(new { message = "Book issue not found." })
            : Ok(ToResponse(issue));
    }

    [HttpPost("issue")]
    [ProducesResponseType(typeof(BookIssueResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookIssueResponse>> Issue(IssueBookRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);

        var book = await dbContext.Books
            .SingleOrDefaultAsync(item => item.Id == request.BookId, cancellationToken);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        var student = await dbContext.Students
            .SingleOrDefaultAsync(item => item.Id == request.StudentId, cancellationToken);
        if (student is null)
        {
            return NotFound(new { message = "Student not found." });
        }

        var hasActiveIssue = await dbContext.BookIssues.AnyAsync(issue =>
            issue.BookId == request.BookId &&
            issue.StudentId == request.StudentId &&
            issue.ReturnedOn == null,
            cancellationToken);
        if (hasActiveIssue)
        {
            return Conflict(new { message = "This student already has an active issue for this book." });
        }

        if (book.Quantity <= 0)
        {
            return Conflict(new { message = "No available copies of this book." });
        }

        var issue = new BookIssue
        {
            BookId = book.Id,
            StudentId = student.Id,
            Book = book,
            Student = student,
            IssuedOn = DateOnly.FromDateTime(DateTime.Today),
            ReturnedOn = null
        };

        book.Quantity--;
        dbContext.BookIssues.Add(issue);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = issue.Id }, ToResponse(issue));
    }

    [HttpPost("{id:int}/return")]
    [ProducesResponseType(typeof(BookIssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookIssueResponse>> Return(int id, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);

        var issue = await dbContext.BookIssues
            .Include(item => item.Book)
            .Include(item => item.Student)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (issue is null)
        {
            return NotFound(new { message = "Book issue not found." });
        }

        if (issue.ReturnedOn is not null)
        {
            return Conflict(new { message = "This book issue has already been returned." });
        }

        issue.ReturnedOn = DateOnly.FromDateTime(DateTime.Today);
        issue.Book.Quantity++;

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Ok(ToResponse(issue));
    }

    private static BookIssueResponse ToResponse(BookIssue issue) => new()
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
    };
}
