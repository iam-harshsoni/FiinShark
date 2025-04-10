using System.Linq.Expressions;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using api.Repository;
using api.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private readonly ApplicationDbContext _db;

        public StockRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var stockToUpdate = await _db.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockToUpdate != null)
            {
                stockToUpdate.Symbol = updateDto.Symbol;
                stockToUpdate.CompanyName = updateDto.CompanyName;
                stockToUpdate.Industry = updateDto.Industry;
                stockToUpdate.Purchase = updateDto.Purchase;
                stockToUpdate.LastDiv = updateDto.LastDiv;
                stockToUpdate.MarketCap = updateDto.MarketCap;
            }

                return stockToUpdate;
        }
    }
}
