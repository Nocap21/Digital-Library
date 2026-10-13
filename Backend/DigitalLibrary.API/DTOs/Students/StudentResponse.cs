namespace DigitalLibrary.API.DTOs.Students;

public class StudentResponse
{
    public int Id { get; set; }
    public string RollNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
}
