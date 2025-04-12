using api.Dtos.Comment;
using api.Mappers;
using api.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allComments = await _unitOfWork.Comment.GetAllAsync();
            var commentDto = allComments.Select(x => x.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _unitOfWork.Comment.GetAsync(x => x.Id == id);

            if (comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (! await _unitOfWork.Stock.isStockExists(stockId))
            {
                return BadRequest();
            }

            var commentModel = commentDto.ToCommentFromCommentDto(stockId);

            await _unitOfWork.Comment.AddAsync(commentModel);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetAll), new { id = commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _unitOfWork.Comment.UpdateAsync(id, commentDto);

            if (commentModel == null) return NotFound();
             
            await _unitOfWork.SaveAsync();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _unitOfWork.Comment.GetAsync(x => x.Id == id);

            if (commentModel == null) return NotFound();

            _unitOfWork.Comment.Remove(commentModel);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
