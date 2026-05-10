namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a fine applied to a patron.
    /// </summary>
    public class Fine
    {
        public int Id { get; set; }
        public int PatronId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateApplied { get; set; }
        public bool IsPaid { get; set; }
        public string Reason { get; set; } = string.Empty;
        
        // Navigation property
        public Patron Patron { get; set; } = null!;
    }
}
