using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data;

public static class DatabaseSchemaUpdater
{
    public static void EnsureLatestSchema(this LibraryDbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw(
            """
            CREATE TABLE IF NOT EXISTS Genres (
                Id INTEGER NOT NULL CONSTRAINT PK_Genres PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                UpdatedAt TEXT NULL
            );
            """);

        dbContext.Database.ExecuteSqlRaw(
            """
            DELETE FROM Genres
            WHERE Id NOT IN (
                SELECT MIN(Id)
                FROM Genres
                GROUP BY lower(Name)
            );
            """);

        dbContext.Database.ExecuteSqlRaw(
            "CREATE UNIQUE INDEX IF NOT EXISTS IX_Genres_Name ON Genres (Name);");
    }
}
