using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class PatronService
{
    private readonly LibraryDbContext _dbContext;

    public PatronService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Patron>> GetAllPatronsAsync()
    {
        return await _dbContext.Patrons
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<Patron?> GetPatronByIdAsync(int id)
    {
        return await _dbContext.Patrons
            .Include(p => p.Transactions)
                .ThenInclude(t => t.Book)
            .Include(p => p.Fines)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patron?> GetPatronByMembershipIdAsync(string membershipId)
    {
        return await _dbContext.Patrons
            .FirstOrDefaultAsync(p => p.MembershipId == membershipId);
    }

    public async Task<List<Patron>> SearchPatronsAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _dbContext.Patrons
            .Where(p => p.FullName.ToLower().Contains(term) ||
                       p.MembershipId.ToLower().Contains(term) ||
                       p.Email.ToLower().Contains(term))
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<List<Patron>> GetActivePatronsAsync()
    {
        return await _dbContext.Patrons
            .Where(p => p.IsActive)
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<Patron> AddPatronAsync(Patron patron)
    {
        patron.CreatedAt = DateTime.UtcNow;
        patron.IsActive = true;
        
        _dbContext.Patrons.Add(patron);
        await _dbContext.SaveChangesAsync();
        return patron;
    }

    public async Task<Patron> UpdatePatronAsync(Patron patron)
    {
        // Detach any existing tracked entity with the same ID
        var existingEntry = _dbContext.ChangeTracker.Entries<Patron>()
            .FirstOrDefault(e => e.Entity.Id == patron.Id);
        
        if (existingEntry != null)
        {
            existingEntry.State = EntityState.Detached;
        }

        patron.UpdatedAt = DateTime.UtcNow;
        
        _dbContext.Patrons.Update(patron);
        await _dbContext.SaveChangesAsync();
        return patron;
    }

    public async Task<bool> DeletePatronAsync(int id)
    {
        var patron = await _dbContext.Patrons.FindAsync(id);
        if (patron == null)
            return false;

        // Check if patron has ANY transactions (active or historical)
        var hasTransactions = await _dbContext.Transactions
            .AnyAsync(t => t.PatronId == id);

        if (hasTransactions)
        {
            throw new InvalidOperationException(
                "Cannot delete this patron because they have transaction history. " +
                "Patrons with checkout records cannot be deleted to maintain data integrity.");
        }

        // Check if patron has ANY fines (paid or unpaid)
        var hasFines = await _dbContext.Fines
            .AnyAsync(f => f.PatronId == id);

        if (hasFines)
        {
            throw new InvalidOperationException(
                "Cannot delete this patron because they have fine records. " +
                "Patrons with fines cannot be deleted to maintain financial records.");
        }

        _dbContext.Patrons.Remove(patron);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsMembershipIdUniqueAsync(string membershipId, int? excludePatronId = null)
    {
        var query = _dbContext.Patrons.Where(p => p.MembershipId == membershipId);
        
        if (excludePatronId.HasValue)
            query = query.Where(p => p.Id != excludePatronId.Value);

        return !await query.AnyAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludePatronId = null)
    {
        var query = _dbContext.Patrons.Where(p => p.Email == email);
        
        if (excludePatronId.HasValue)
            query = query.Where(p => p.Id != excludePatronId.Value);

        return !await query.AnyAsync();
    }

    public async Task<decimal> GetTotalUnpaidFinesAsync(int patronId)
    {
        return await _dbContext.Fines
            .Where(f => f.PatronId == patronId && !f.IsPaid)
            .SumAsync(f => f.Amount);
    }
}
