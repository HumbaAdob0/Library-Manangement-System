namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a book checkout/return transaction.
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; } = "Checked Out"; // Checked Out, Returned, Overdue
        
        // Navigation properties
        public Book Book { get; set; } = null!;
        public Patron Patron { get; set; } = null!;
    }
}
