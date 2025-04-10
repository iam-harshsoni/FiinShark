using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public StockController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _db.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks); // 200 Status Code.
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (id == 0) return BadRequest();

            var stock = await _db.Stocks.FindAsync(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());  // 200 Status Code
        }

        [HttpPost("createStockDto")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();

            await _db.Stocks.AddAsync(stockModel);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = stockModel.Id }, stockModel.ToStockDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockToUpdate = await _db.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockToUpdate == null) return NotFound();

            stockToUpdate.Symbol = updateDto.Symbol;
            stockToUpdate.CompanyName = updateDto.CompanyName;
            stockToUpdate.Industry = updateDto.Industry;
            stockToUpdate.Purchase = updateDto.Purchase;
            stockToUpdate.LastDiv = updateDto.LastDiv;
            stockToUpdate.MarketCap = updateDto.MarketCap;

            await _db.SaveChangesAsync();

            return Ok(stockToUpdate.ToStockDto());  // 200 Status code
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id == 0 || id <= 0) return BadRequest();

            var stockToDelete = await _db.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockToDelete == null) return NotFound();

            _db.Stocks.Remove(stockToDelete);
            await _db.SaveChangesAsync();

            return NoContent();  //204 Status Code
        }

        [HttpGet("countStocks")]
        public IActionResult CountStock()
        {
            var numOfStock = _db.Stocks.Count();

            return Ok(numOfStock);
        }

    }
}
