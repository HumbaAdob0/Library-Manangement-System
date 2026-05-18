using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class FineService
{
    private readonly LibraryDbContext _dbContext;

    public FineService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Fine>> GetAllFinesAsync()
    {
        return await _dbContext.Fines
            .Include(f => f.Patron)
            .OrderByDescending(f => f.DateApplied)
            .ToListAsync();
    }

    public async Task<Fine?> GetFineByIdAsync(int id)
    {
        return await _dbContext.Fines
            .Include(f => f.Patron)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<Fine>> GetPatronFinesAsync(int patronId)
    {
        return await _dbContext.Fines
            .Where(f => f.PatronId == patronId)
            .OrderByDescending(f => f.DateApplied)
            .ToListAsync();
    }

    public async Task<List<Fine>> GetUnpaidFinesAsync()
    {
        return await _dbContext.Fines
            .Include(f => f.Patron)
            .Where(f => !f.IsPaid)
            .OrderByDescending(f => f.DateApplied)
            .ToListAsync();
    }

    public async Task<List<Fine>> GetPatronUnpaidFinesAsync(int patronId)
    {
        return await _dbContext.Fines
            .Where(f => f.PatronId == patronId && !f.IsPaid)
            .OrderByDescending(f => f.DateApplied)
            .ToListAsync();
    }

    public async Task<Fine> AddFineAsync(Fine fine)
    {
        fine.CreatedAt = DateTime.UtcNow;
        fine.DateApplied = DateTime.UtcNow;
        fine.IsPaid = false;
        
        _dbContext.Fines.Add(fine);
        await _dbContext.SaveChangesAsync();
        return fine;
    }

    public async Task<Fine> PayFineAsync(int fineId)
    {
        var fine = await _dbContext.Fines.FindAsync(fineId);
        if (fine == null)
            throw new InvalidOperationException("Fine not found.");

        if (fine.IsPaid)
            throw new InvalidOperationException("Fine already paid.");

        fine.IsPaid = true;
        fine.DatePaid = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return fine;
    }

    public async Task<bool> PayAllPatronFinesAsync(int patronId)
    {
        var unpaidFines = await GetPatronUnpaidFinesAsync(patronId);
        
        if (!unpaidFines.Any())
            return false;

        foreach (var fine in unpaidFines)
        {
            fine.IsPaid = true;
            fine.DatePaid = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalUnpaidFinesAsync(int patronId)
    {
        return await _dbContext.Fines
            .Where(f => f.PatronId == patronId && !f.IsPaid)
            .SumAsync(f => f.Amount);
    }

    public async Task<decimal> GetTotalPaidFinesAsync(int patronId)
    {
        return await _dbContext.Fines
            .Where(f => f.PatronId == patronId && f.IsPaid)
            .SumAsync(f => f.Amount);
    }

    public async Task<bool> DeleteFineAsync(int id)
    {
        var fine = await _dbContext.Fines.FindAsync(id);
        if (fine == null)
            return false;

        _dbContext.Fines.Remove(fine);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
