using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using api.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public StockController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _unitOfWork.Stock.GetAllAsync(includeProperties: "Comments");
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto); // 200 Status Code.
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (id == 0) return BadRequest();

            var stock = await _unitOfWork.Stock.GetAsync(x => x.Id == id, includeProperties: "Comments");

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());  // 200 Status Code
        }

        [HttpPost("createStockDto")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();

            await _unitOfWork.Stock.AddAsync(stockModel);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetAll), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            Stock? stockToUpdate = await _unitOfWork.Stock.UpdateAsync(id, updateDto);

            if (stockToUpdate == null) return NotFound();

            await _unitOfWork.SaveAsync();

            return Ok(stockToUpdate.ToStockDto());  // 200 Status code
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id == 0 || id <= 0) return BadRequest();

            var stockToDelete = await _unitOfWork.Stock.GetAsync(x => x.Id == id, includeProperties: "Comments");

            if (stockToDelete == null) return NotFound();

            _unitOfWork.Stock.Remove(stockToDelete);
            await _unitOfWork.SaveAsync();

            return NoContent();  //204 Status Code
        }


    }
}
