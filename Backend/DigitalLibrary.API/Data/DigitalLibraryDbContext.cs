using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.API.Data;

public class DigitalLibraryDbContext(DbContextOptions<DigitalLibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<BookIssue> BookIssues => Set<BookIssue>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books", table => table.HasCheckConstraint("CK_Books_Quantity_NonNegative", "[Quantity] >= 0"));
            entity.HasKey(book => book.Id);
            entity.Property(book => book.Id).UseIdentityColumn();
            entity.Property(book => book.ISBN).HasColumnType("varchar(50)").IsRequired();
            entity.HasIndex(book => book.ISBN).IsUnique();
            entity.Property(book => book.Name).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(book => book.Author).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(book => book.Publication).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(book => book.Details).HasColumnType("nvarchar(250)").IsRequired();
            entity.Property(book => book.Quantity).IsRequired();
            entity.Property(book => book.Price).HasPrecision(10, 2).IsRequired();
            entity.Property(book => book.Branch).HasColumnType("nvarchar(20)").IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(student => student.Id);
            entity.Property(student => student.Id).UseIdentityColumn();
            entity.Property(student => student.RollNo).HasColumnType("nvarchar(50)").IsRequired();
            entity.HasIndex(student => student.RollNo).IsUnique();
            entity.Property(student => student.Name).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(student => student.Branch).HasColumnType("nvarchar(20)").IsRequired();
        });

        modelBuilder.Entity<BookIssue>(entity =>
        {
            entity.ToTable("BookIssues");
            entity.HasKey(issue => issue.Id);
            entity.Property(issue => issue.Id).UseIdentityColumn();
            entity.Property(issue => issue.BookId).IsRequired();
            entity.Property(issue => issue.StudentId).IsRequired();
            entity.Property(issue => issue.IssuedOn).HasColumnType("date").IsRequired();
            entity.Property(issue => issue.ReturnedOn).HasColumnType("date");

            entity.HasOne(issue => issue.Book)
                .WithMany(book => book.BookIssues)
                .HasForeignKey(issue => issue.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(issue => issue.Student)
                .WithMany(student => student.BookIssues)
                .HasForeignKey(issue => issue.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Id).UseIdentityColumn();
            entity.Property(user => user.Username).HasMaxLength(50).IsRequired();
            entity.HasIndex(user => user.Username).IsUnique();
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.CreatedAt).IsRequired();
        });
    }
}
