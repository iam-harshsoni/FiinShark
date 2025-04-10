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
            /*
                Select is a dotnet version of map which is in jS.
                we are transferring every record to the StockDto from Stock 
             */

            // Here we are getting all the records with all the fields of Stock Model,
            // getting it in List() and then mapping it to Stockdto using Select(z=>z.toStockDto)
            // so we will get only those fields that are available in StockDTO.

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
            // whatever record we got in stock, will be transferred into ToStockDTO and we are returning it with OK (200) status code. 
            // In result, we will get 200 OK response, with the json data containing all the properties of StockDTO.
        }

        [HttpPost("createStockDto")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // This below code ToStockFromCreateDto() method is 'Extension method' which converts 'CreateDto' data which are received from Body of
            // 'HTTPPost Creat method 'into 'Stock Model' and store it in 'stockModel Variable'

            var stockModel = stockDto.ToStockFromCreateDto(); 

            await _db.Stocks.AddAsync(stockModel);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new {id = stockModel.Id }, stockModel.ToStockDto());  //201 Status code, Mostly used for Post(Create New Record)

            // Explainination of last line -  return CreatedAtAction(nameof(GetAll), stockModel.Id, stockModel.ToStockDto());
            // 1. Returns HTTP 201 Created status to indicate successful creation of the resource.
            // 2. Uses nameof(GetAll) to set the Location header pointing to the related endpoint (usually should be GetById).
            // 3. Sends the created stock data (converted to DTO) in the response body to avoid exposing internal entity details.
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

            //return CreatedAtAction(nameof(GetById), new { id = id}, stockToUpdate.ToStockDto());
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
