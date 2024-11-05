using Core.Data.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class PostReactionsService : IPostReactionsService
    {
        private readonly IPostReactionsRepository _postReactionsRepository;
        private readonly Guid UserId;

        public PostReactionsService(IPostReactionsRepository postReactionsRepository, ICurrentUserService currentUserService)
        {
            _postReactionsRepository = postReactionsRepository;
            UserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }


        public async Task AddPostReactionAsync(Guid postId, ReactionType reaction)
        {
            var postReaction = new PostReaction()
            {
                PostId = postId,
                UserId = UserId,
                Reaction = reaction
            };

            await _postReactionsRepository.AddPostReactionAsync(postReaction);

            if (! await _postReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post reaction to database.");
            }
        }

        public async Task UpdatePostReaction(Guid postId)
        {
            if (!await _postReactionsRepository.PostReactionExistsAsync(UserId, postId))
            {
                throw new NotFoundException($"Post reaction not found, PostID: {postId} UserID: {UserId}");
            }

            PostReaction postReaction = await _postReactionsRepository.GetPostReactionByIdAsync(UserId, postId);

            _postReactionsRepository.UpdatePostReaction(postReaction);

            if (!await _postReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated post reaction to database.");
            }
        }

        public async Task DeletePostReaction(Guid postId)
        {
            if (!await _postReactionsRepository.PostReactionExistsAsync(UserId, postId))
            {
                throw new NotFoundException($"Post reaction not found, PostID: {postId} UserID: {UserId}");
            }

            PostReaction postReaction = await _postReactionsRepository.GetPostReactionByIdAsync(UserId, postId);

            _postReactionsRepository.DeletePostReaction(postReaction);

            if (!await _postReactionsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save post reaction deletion to database.");
            }
        }
    }
}
