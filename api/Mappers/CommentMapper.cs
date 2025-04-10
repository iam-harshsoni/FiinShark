using System.Diagnostics;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreateOn = commentModel.CreateOn,
                StockId = commentModel.StockId
            };
        }

        public static Comment ToCommentFromCommentDto(this CreateCommentRequestDto commentDto)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                CreateOn = commentDto.CreateOn,
                StockId = commentDto.StockId
            };
        }

        public static Comment ToStockFromUpdateDto(this UpdateCommentRequestDto commentDto)
        {
            return new Comment
            { 
                Title = commentDto.Title,
                Content = commentDto.Content,
                CreateOn = commentDto.CreateOn,
                StockId = commentDto.StockId
            };
        }
    }
}
