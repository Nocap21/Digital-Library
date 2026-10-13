using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs.Students;
using DigitalLibrary.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class StudentsController(DigitalLibraryDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var students = await dbContext.Students
            .AsNoTracking()
            .OrderBy(student => student.Id)
            .ToListAsync(cancellationToken);

        return Ok(students.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var student = await dbContext.Students.AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return student is null
            ? NotFound(new { message = "Student not found." })
            : Ok(ToResponse(student));
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentResponse>> Create(CreateStudentRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.Students.AnyAsync(student => student.RollNo == request.RollNo, cancellationToken))
        {
            return DuplicateRollNo();
        }

        var student = new Student
        {
            RollNo = request.RollNo,
            Name = request.Name,
            Branch = request.Branch
        };

        dbContext.Students.Add(student);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            return DuplicateRollNo();
        }

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, ToResponse(student));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentResponse>> Update(int id, UpdateStudentRequest request, CancellationToken cancellationToken)
    {
        var student = await dbContext.Students.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (student is null)
        {
            return NotFound(new { message = "Student not found." });
        }

        if (await dbContext.Students.AnyAsync(item => item.RollNo == request.RollNo && item.Id != id, cancellationToken))
        {
            return DuplicateRollNo();
        }

        student.RollNo = request.RollNo;
        student.Name = request.Name;
        student.Branch = request.Branch;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            return DuplicateRollNo();
        }

        return Ok(ToResponse(student));
    }

    private static ObjectResult DuplicateRollNo() => new ConflictObjectResult(
        new { message = "A student with this Roll No. already exists." });

    private static bool IsUniqueConstraintViolation(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (current is SqlException { Number: 2601 or 2627 })
            {
                return true;
            }
        }

        return false;
    }

    private static StudentResponse ToResponse(Student student) => new()
    {
        Id = student.Id,
        RollNo = student.RollNo,
        Name = student.Name,
        Branch = student.Branch
    };
}
