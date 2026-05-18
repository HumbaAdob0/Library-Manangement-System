namespace LibraryManagementSystem.Models;

public class Transaction
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int PatronId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal FineAmount { get; set; }
    public bool IsReturned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Book Book { get; set; } = null!;
    public Patron Patron { get; set; } = null!;
}
