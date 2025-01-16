using Core.Data.Dto.CommentDto;
using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.Extensions.Hosting;
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
        private readonly IProfileRepository _profileRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly Guid _currentUserId;

        public CommentsService(ICommentsRepository commentsRepository, IPostsRepository postsRepository,
            IProfileRepository profileRepository ,ICurrentUserService currentUserService)
        {
            _commentsRepository = commentsRepository;
            _postsRepository = postsRepository;
            _profileRepository = profileRepository;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }

        // Gets comments by post id
        public async Task<List<CommentResponse>> GetCommentsByPostIdAsync(Guid postId)
        {
            List<Comment> comments = await _commentsRepository.GetCommentsByPostIdAsync(postId);

            var commentResponses = comments.Select(c => c.ToCommentResponse(_currentUserId)).ToList();

            return commentResponses;
        }

        // Adds a comment to database
        public async Task<CommentResponse> AddCommentAsync(Guid postId, CommentAddRequest commentAddRequest)
        {
            Comment comment = commentAddRequest.ToComment(_currentUserId, postId);

            Post post = await _postsRepository.GetPostByIdAsync(postId)
                ?? throw new NotFoundException($"Post not found, PostID: {postId}");

            ++post.CommentCount;
            post.Comments.Add(comment);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added comment to database.");
            }

            comment.User = await _profileRepository.GetProfileByIdAsync(_currentUserId);

            return comment.ToCommentResponse(_currentUserId);
        }

        // Adds a comment reply to database
        public async Task<CommentReplyDto> AddCommentReplyAsync(Guid commentId, CommentAddRequest commentAddRequest)
        {
            Comment parentComment = await _commentsRepository.GetCommentByIdAsync(commentId)
                ?? throw new NotFoundException($"Comment not found, Comment ID: {commentId}");

            Comment commentReply = commentAddRequest.ToComment(_currentUserId, parentComment.PostId, commentId);

            parentComment.CommentReplies.Add(commentReply);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added comment to database.");
            }

            commentReply.User = await _profileRepository.GetProfileByIdAsync(_currentUserId);

            return commentReply.ToCommentReply(_currentUserId);
        }

        // Updates comment in database with new information
        public async Task UpdateCommentAsync(Guid commentId, CommentUpdateRequest updatedCommentRequest)
        {
            Comment existingComment = await _commentsRepository.GetCommentByIdAsync(commentId)
                ?? throw new NotFoundException($"Comment not found, Comment ID: {commentId}");

            Comment updatedComment = updatedCommentRequest.ToComment(existingComment);

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
            Comment comment = await _commentsRepository.GetCommentByIdAsync(commentId)
                ?? throw new NotFoundException($"Comment not found, Comment ID: {commentId}");

            if (comment.UserId != _currentUserId && comment.Post!.UserId != _currentUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this comment.");
            }

            Post post = await _postsRepository.GetPostByIdAsync(comment.PostId)
                ?? throw new NotFoundException($"Post not found, ID: {comment.PostId}");

            --post.CommentCount;
            post.Comments.Remove(comment);

            if (!await _commentsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save comment deletion to database.");
            }
        }
    }
}
