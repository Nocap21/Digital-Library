using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs.Books;
using DigitalLibrary.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BooksController(DigitalLibraryDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await dbContext.Books
            .AsNoTracking()
            .OrderBy(book => book.Id)
            .ToListAsync(cancellationToken);

        return Ok(books.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return book is null ? NotFound(new { message = "Book not found." }) : Ok(ToResponse(book));
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.Books.AnyAsync(book => book.ISBN == request.ISBN, cancellationToken))
        {
            return Conflict(new { message = "A book with this ISBN already exists." });
        }

        var book = new Book
        {
            ISBN = request.ISBN,
            Name = request.Name,
            Author = request.Author,
            Publication = request.Publication,
            Details = request.Details,
            Quantity = request.Quantity,
            Price = request.Price,
            Branch = request.Branch
        };

        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = ToResponse(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookResponse>> Update(int id, UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        if (await dbContext.Books.AnyAsync(item => item.ISBN == request.ISBN && item.Id != id, cancellationToken))
        {
            return Conflict(new { message = "A book with this ISBN already exists." });
        }

        book.ISBN = request.ISBN;
        book.Name = request.Name;
        book.Author = request.Author;
        book.Publication = request.Publication;
        book.Details = request.Details;
        book.Quantity = request.Quantity;
        book.Price = request.Price;
        book.Branch = request.Branch;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(ToResponse(book));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        if (await dbContext.BookIssues.AnyAsync(issue => issue.BookId == id, cancellationToken))
        {
            return Conflict(new { message = "This book cannot be deleted because it is referenced by an issue record." });
        }

        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static BookResponse ToResponse(Book book) => new()
    {
        Id = book.Id,
        ISBN = book.ISBN,
        Name = book.Name,
        Author = book.Author,
        Publication = book.Publication,
        Details = book.Details,
        Quantity = book.Quantity,
        Price = book.Price,
        Branch = book.Branch
    };
}
