using api.Dtos.Comment;
using api.Models;

namespace api.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto);
    }
}
