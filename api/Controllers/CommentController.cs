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
            var allComments = await _unitOfWork.Comment.GetAllAsync();
            var commentDto = allComments.Select(x => x.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int? id)
        {
            var comment = await _unitOfWork.Comment.GetAsync(x => x.Id == id);

            if (comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestDto commentDto)
        {
            var commentModel = commentDto.ToCommentFromCommentDto();

            await _unitOfWork.Comment.AddAsync(commentModel);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetAll), new { id = commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, UpdateCommentRequestDto commentDto)
        {
            var commentModel = await _unitOfWork.Comment.Update(id, commentDto);

            if (commentModel == null) return NotFound();
             
            await _unitOfWork.SaveAsync();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute]int id)
        {
            var commentModel = await _unitOfWork.Comment.GetAsync(x => x.Id == id, includeProperties: "Comment");

            if (commentModel == null) return NotFound();

            _unitOfWork.Comment.Remove(commentModel);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
