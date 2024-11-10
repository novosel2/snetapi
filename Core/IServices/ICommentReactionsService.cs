using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ICommentReactionsService
    {
        /// <summary>
        /// Adds a comment reaction to the database
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <param name="reaction">Reaction</param>
        public Task AddCommentReactionAsync(Guid commentId, ReactionType reaction);

        /// <summary>
        /// Deletes comment reaction from database
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        public Task DeleteCommentReaction(Guid commentId);

        /// <summary>
        /// Updates comment reaction 
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        public Task UpdateCommentReaction(Guid commentId);
    }
}
