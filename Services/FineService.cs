using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class FineService : IFineService
    {
        private readonly LibraryDbContext _context;

        public FineService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fine>> GetAllFinesAsync()
        {
            return await _context.Fines
                .Include(f => f.Patron)
                .OrderByDescending(f => f.DateApplied)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fine>> GetFinesByPatronAsync(int patronId)
        {
            return await _context.Fines
                .Where(f => f.PatronId == patronId)
                .OrderByDescending(f => f.DateApplied)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync()
        {
            return await _context.Fines
                .Include(f => f.Patron)
                .Where(f => !f.IsPaid)
                .ToListAsync();
        }

        public async Task AddFineAsync(Fine fine)
        {
            fine.DateApplied = DateTime.Now;
            fine.IsPaid = false;
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();
        }

        public async Task MarkFineAsPaidAsync(int fineId)
        {
            var fine = await _context.Fines.FindAsync(fineId);
            if (fine != null)
            {
                fine.IsPaid = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalUnpaidFinesByPatronAsync(int patronId)
        {
            return await _context.Fines
                .Where(f => f.PatronId == patronId && !f.IsPaid)
                .SumAsync(f => f.Amount);
        }
    }
}
