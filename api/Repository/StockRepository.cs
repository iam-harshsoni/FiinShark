using System.Linq.Expressions;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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

        public Task<bool> isStockExists(int id)
        {
            return _db.Stocks.AnyAsync(x=>x.Id == id);
        }

        

        public async Task<IEnumerable<Stock?>> GetAllAsync(QueryObject query,string? includeProperties = null)
        {
            IQueryable<Stock> stock = dbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    stock = stock.Include(property);
                }
            }

            // Searching based on Symbol or Company Name

            //  var stock = querys.AsQueryable();

            //if (!string.IsNullOrWhiteSpace(query.CompanyName)){
            //    querys = querys.Where(x => x.CompanyName.Contains(query.CompanyName));
            //}


            //if (!string.IsNullOrWhiteSpace(query.Symbol)){
            //    querys = querys.Where(x => x.Symbol == query.Symbol);
            //}


            // This code used 'OR' for query. Above code is 'AND' operation and returning null if any of the value is miss matched.  
            if (!string.IsNullOrWhiteSpace(query.CompanyName) || !string.IsNullOrWhiteSpace(query.Symbol))
            {
                stock = stock.Where(x =>
                    (!string.IsNullOrWhiteSpace(query.CompanyName) && x.CompanyName.Contains(query.CompanyName)) ||
                    (!string.IsNullOrWhiteSpace(query.Symbol) && x.Symbol == query.Symbol));
            }


            // Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(x => x.Symbol);
                }
            }


            //pagination functionlity using Skip().Take()
            var skipNumber = (query.PageNumber - 1) * query.PageSize;


            return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }
    }
}
