using api.Dtos.Stock;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);
    }
}
