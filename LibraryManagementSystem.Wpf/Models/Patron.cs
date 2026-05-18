namespace LibraryManagementSystem.Models;

public class Patron
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string MembershipId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public MembershipType MembershipType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
