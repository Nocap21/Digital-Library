namespace DigitalLibrary.API.DTOs.Dashboard;

public class DashboardResponse
{
    public int TotalBooks { get; set; }
    public int TotalStudents { get; set; }
    public int CurrentlyIssuedBooks { get; set; }
    public int AvailableCopies { get; set; }
    public List<IssuedBookSummaryResponse> IssuedBooks { get; set; } = [];
}
