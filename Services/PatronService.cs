using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class PatronService : IPatronService
    {
        private readonly LibraryDbContext _context;

        public PatronService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patron>> GetAllPatronsAsync()
        {
            return await _context.Patrons.ToListAsync();
        }

        public async Task<Patron?> GetPatronByIdAsync(int id)
        {
            return await _context.Patrons.FindAsync(id);
        }

        public async Task<Patron?> GetPatronByMembershipIdAsync(string membershipId)
        {
            return await _context.Patrons
                .FirstOrDefaultAsync(p => p.MembershipId == membershipId);
        }

        public async Task<IEnumerable<Patron>> SearchPatronsAsync(string searchTerm)
        {
            return await _context.Patrons
                .Where(p => p.FullName.Contains(searchTerm) ||
                           p.MembershipId.Contains(searchTerm) ||
                           p.Email.Contains(searchTerm) ||
                           p.PhoneNumber.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task AddPatronAsync(Patron patron)
        {
            patron.JoinDate = DateTime.Now;
            patron.IsActive = true;
            _context.Patrons.Add(patron);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatronAsync(Patron patron)
        {
            _context.Patrons.Update(patron);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatronAsync(int id)
        {
            var patron = await _context.Patrons.FindAsync(id);
            if (patron != null)
            {
                _context.Patrons.Remove(patron);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateMembershipIdAsync()
        {
            var lastPatron = await _context.Patrons
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            int nextNumber = (lastPatron?.Id ?? 0) + 1;
            return $"LIB{DateTime.Now.Year}{nextNumber:D5}";
        }
    }
}

