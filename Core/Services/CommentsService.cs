using Core.Data.Dto.CommentDto;
using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly Guid _currentUserId;

        public CommentsService(ICommentsRepository commentsRepository, ICurrentUserService currentUserService)
        {
            _commentsRepository = commentsRepository;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }

        // Adds a comment to database
        public async Task AddCommentAsync(Guid postId, CommentAddRequest commentAddRequest)
        {
            Comment comment = commentAddRequest.ToComment(_currentUserId, postId);

            await _commentsRepository.AddCommentAsync(comment);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added comment to database.");
            }
        }

        // Updates comment in database with new information
        public async Task UpdateCommentAsync(Guid commentId, CommentUpdateRequest updatedCommentRequest)
        {
            if (!await _commentsRepository.CommentExistsAsync(commentId))
            {
                throw new NotFoundException($"Comment not found, Comment ID: {commentId}");
            }

            Comment existingComment = await _commentsRepository.GetCommentByIdAsync(commentId);
            Comment updatedComment = updatedCommentRequest.ToComment(existingComment.Id, _currentUserId, existingComment.PostId);

            if (existingComment.UserId != _currentUserId)
            {
                throw new UnauthorizedException("You do not have permission to update this comment.");
            }

            _commentsRepository.UpdateComment(existingComment, updatedComment);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated comment to database.");
            }
        }

        // Deletes comment from database
        public async Task DeleteCommentAsync(Guid commentId)
        {
            if (!await _commentsRepository.CommentExistsAsync(commentId))
            {
                throw new NotFoundException($"Comment not found, Comment ID: {commentId}");
            }

            Comment comment = await _commentsRepository.GetCommentByIdAsync(commentId);

            if (comment.UserId != _currentUserId && comment.Post!.UserId != _currentUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this comment.");
            }

            _commentsRepository.DeleteComment(comment);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save comment deletion to database.");
            }
        }
    }
}
