using api.Data;
using api.Dtos.Comment;
using api.Models;
using api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentReposiotry : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _db;

        public CommentReposiotry(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Comment?> Update(int id, UpdateCommentRequestDto commentDto)
        {
            var commentToUpdate = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (commentToUpdate != null)
            {
                commentToUpdate.Title = commentDto.Title;
                commentToUpdate.Content = commentDto.Content;
                commentToUpdate.StockId = commentDto.StockId;
                commentToUpdate.CreateOn = commentDto.CreateOn;

            }

            return commentToUpdate;
        }
    }
}
