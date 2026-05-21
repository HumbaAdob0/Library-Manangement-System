using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class GenreService
{
    private readonly LibraryDbContext _dbContext;

    public GenreService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _dbContext.Genres
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        return await _dbContext.Genres.FindAsync(id);
    }

    public async Task<Genre> AddGenreAsync(Genre genre)
    {
        genre.Name = genre.Name.Trim();
        genre.CreatedAt = DateTime.UtcNow;
        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre> UpdateGenreAsync(Genre genre)
    {
        var existingGenre = await _dbContext.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == genre.Id);

        if (existingGenre == null)
        {
            throw new InvalidOperationException("Genre not found.");
        }

        var oldName = existingGenre.Name;

        var existingEntry = _dbContext.ChangeTracker.Entries<Genre>()
            .FirstOrDefault(e => e.Entity.Id == genre.Id);
        
        if (existingEntry != null)
        {
            existingEntry.State = EntityState.Detached;
        }

        genre.Name = genre.Name.Trim();
        genre.CreatedAt = existingGenre.CreatedAt;
        genre.UpdatedAt = DateTime.UtcNow;
        _dbContext.Genres.Update(genre);

        if (!string.Equals(oldName, genre.Name, StringComparison.Ordinal))
        {
            var books = await _dbContext.Books
                .Where(b => b.Genre == oldName)
                .ToListAsync();

            foreach (var book in books)
            {
                book.Genre = genre.Name;
                book.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task<bool> DeleteGenreAsync(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null)
            return false;

        // Check if genre is used by any books
        var hasBooks = await _dbContext.Books
            .AnyAsync(b => b.Genre == genre.Name);

        if (hasBooks)
        {
            throw new InvalidOperationException(
                $"Cannot delete genre '{genre.Name}' because it is used by one or more books.");
        }

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsGenreNameUniqueAsync(string name, int? excludeGenreId = null)
    {
        name = name.Trim();
        var query = _dbContext.Genres.Where(g => g.Name.ToLower() == name.ToLower());
        
        if (excludeGenreId.HasValue)
            query = query.Where(g => g.Id != excludeGenreId.Value);

        return !await query.AnyAsync();
    }
}
