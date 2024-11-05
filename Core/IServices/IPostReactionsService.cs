using Core.Enums;

namespace Core.IServices
{
    public interface IPostReactionsService
    {
        /// <summary>
        /// Adds a post reaction to the database
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="reaction">Reaction</param>
        public Task AddPostReactionAsync(Guid postId, ReactionType reaction);

        /// <summary>
        /// Deletes post reaction from database
        /// </summary>
        /// <param name="postId">Id of post</param>
        public Task DeletePostReaction(Guid postId);

        /// <summary>
        /// Updates post reaction 
        /// </summary>
        /// <param name="postId">Id of post</param>
        public Task UpdatePostReaction(Guid postId);
    }
}
