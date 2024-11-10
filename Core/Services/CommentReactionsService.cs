using Core.Data.Entities;
using Core.Enums;
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
    public class CommentReactionsService : ICommentReactionsService
    {
        private readonly ICommentReactionsRepository _commentReactionsRepository;
        private readonly Guid UserId;

        public CommentReactionsService(ICommentReactionsRepository commentReactionsRepository, ICurrentUserService currentUserService)
        {
            _commentReactionsRepository = commentReactionsRepository;
            UserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }


        public async Task AddCommentReactionAsync(Guid commentId, ReactionType reaction)
        {
            if (await _commentReactionsRepository.CommentReactionExistsAsync(UserId, commentId))
            {
                throw new AlreadyExistsException($"Post reaction on post already exists.");
            }

            var commentReaction = new CommentReaction()
            {
                CommentId = commentId,
                UserId = UserId,
                Reaction = reaction
            };

            await _commentReactionsRepository.AddCommentReactionAsync(commentReaction);

            if (!await _commentReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post reaction to database.");
            }
        }

        public async Task UpdateCommentReaction(Guid commentId)
        {
            if (!await _commentReactionsRepository.CommentReactionExistsAsync(UserId, commentId))
            {
                throw new NotFoundException($"Post reaction not found, PostID: {commentId} UserID: {UserId}");
            }

            CommentReaction commentReaction = await _commentReactionsRepository.GetCommentReactionByIdAsync(UserId, commentId);

            _commentReactionsRepository.UpdateCommentReaction(commentReaction);

            if (!await _commentReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated post reaction to database.");
            }
        }

        public async Task DeleteCommentReaction(Guid commentId)
        {
            if (!await _commentReactionsRepository.CommentReactionExistsAsync(UserId, commentId))
            {
                throw new NotFoundException($"Post reaction not found, PostID: {commentId} UserID: {UserId}");
            }

            CommentReaction commentReaction = await _commentReactionsRepository.GetCommentReactionByIdAsync(UserId, commentId);

            _commentReactionsRepository.DeleteCommentReaction(commentReaction);

            if (!await _commentReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save post reaction deletion to database.");
            }
        }
    }
}
