using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs.Students;

public class UpdateStudentRequest
{
    private string _rollNo = string.Empty;
    private string _name = string.Empty;
    private string _branch = string.Empty;

    [Required, StringLength(50)]
    public string RollNo { get => _rollNo; set => _rollNo = value?.Trim() ?? string.Empty; }

    [Required, StringLength(100)]
    public string Name { get => _name; set => _name = value?.Trim() ?? string.Empty; }

    [Required, RegularExpression("^(IT|MECH|CIVIL)$", ErrorMessage = "Branch must be IT, MECH, or CIVIL.")]
    public string Branch { get => _branch; set => _branch = value?.Trim() ?? string.Empty; }
}
