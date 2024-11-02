using Core.Data.Entities;
using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {


        public Task<List<Post>> GetPostsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostByIdAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPostsByProfileIdAsync(Guid profileId)
        {
            throw new NotImplementedException();
        }

        public Task AddPostAsync(Post post)
        {
            throw new NotImplementedException();
        }

        public void UpdatePost(Post existingPost, Post updatedPost)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostExistsAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSavedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
