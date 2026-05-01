namespace Library_Management_System.Models
{
    /// <summary>
    /// Represents a member of the library.
    /// </summary>
    public class Member
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
    }
}
