namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a patron (library member) in the system.
    /// </summary>
    public class Patron
    {
        public int Id { get; set; }
        public string MembershipId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string MembershipType { get; set; } = "Standard"; // Standard or Premium
        public bool IsActive { get; set; } = true;
        public DateTime JoinDate { get; set; }
        
        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}
