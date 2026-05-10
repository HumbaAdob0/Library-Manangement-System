using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IFineService
    {
        Task<IEnumerable<Fine>> GetAllFinesAsync();
        Task<IEnumerable<Fine>> GetFinesByPatronAsync(int patronId);
        Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
        Task AddFineAsync(Fine fine);
        Task MarkFineAsPaidAsync(int fineId);
        Task<decimal> GetTotalUnpaidFinesByPatronAsync(int patronId);
    }
}
