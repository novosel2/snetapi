using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IPostRepository
    {
        public Task<List<Post>> GetPostsAsync();

        public Task<Post> GetPostByIdAsync(Guid postId);

        public Task<List<Post>> GetPostsByProfileIdAsync(Guid profileId);

        public Task AddPostAsync(Post post);

        public void UpdatePost(Post existingPost, Post updatedPost);

        public Task DeletePostAsync(Guid postId);

        public Task<bool> PostExistsAsync(Guid postId);

        public Task<bool> IsSavedAsync();
    }
}
