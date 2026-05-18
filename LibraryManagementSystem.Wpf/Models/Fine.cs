namespace LibraryManagementSystem.Models;

public class Fine
{
    public int Id { get; set; }
    public int PatronId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateApplied { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? DatePaid { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Patron Patron { get; set; } = null!;
}
