using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs.Books;

public class UpdateBookRequest
{
    private string _isbn = string.Empty;
    private string _name = string.Empty;
    private string _author = string.Empty;
    private string _publication = string.Empty;
    private string _details = string.Empty;
    private string _branch = string.Empty;

    [Required, StringLength(50)]
    public string ISBN { get => _isbn; set => _isbn = value?.Trim() ?? string.Empty; }

    [Required, StringLength(50)]
    public string Name { get => _name; set => _name = value?.Trim() ?? string.Empty; }

    [Required, StringLength(50)]
    public string Author { get => _author; set => _author = value?.Trim() ?? string.Empty; }

    [Required, StringLength(50)]
    public string Publication { get => _publication; set => _publication = value?.Trim() ?? string.Empty; }

    [Required, StringLength(250)]
    public string Details { get => _details; set => _details = value?.Trim() ?? string.Empty; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal Price { get; set; }

    [Required, RegularExpression("^(IT|Civil|Mechanical)$")]
    public string Branch { get => _branch; set => _branch = value?.Trim() ?? string.Empty; }
}
