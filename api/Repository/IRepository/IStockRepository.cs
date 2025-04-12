using System.Linq.Expressions;
using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);
        Task<IEnumerable<Stock?>> GetAllAsync(QueryObject query, string? includeProperties = null);
        Task<bool> isStockExists(int id);
    }
}
