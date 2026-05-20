namespace LibraryManagementSystem.Models;

public class Settings
{
    public string LibraryName { get; set; } = "Library";
    public int DefaultLoanPeriodDays { get; set; } = 14;
    public decimal FinePerDay { get; set; } = 0.50m;
}
