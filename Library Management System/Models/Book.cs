namespace Library_Management_System.Models
{
    /// <summary>
    /// Represents a book in the library system.
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int PublicationYear { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
