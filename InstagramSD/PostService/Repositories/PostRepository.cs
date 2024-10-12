using Domain.Models;
using Microsoft.EntityFrameworkCore;
using PostService.Data;

namespace PostService.Repositories
{
    public class PostRepository : IPostRepository
	{
        private readonly AppDbContext _dbContext;

        public PostRepository(AppDbContext appDbContext)
		{
            _dbContext = appDbContext;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post is null)
            {
                return false;
            }

            _dbContext.Posts.Remove(post);
            return await removeItem();
        }

        private async Task<bool> removeItem()
        {
            return (await _dbContext.SaveChangesAsync()) == 1;
        }

        public async Task<List<Post>> GetPostInformationByPostIds(List<Guid> postIds)
        {
            return await _dbContext.Posts.Where(p => postIds.Contains(p.PostId)).ToListAsync();
        }

        public async Task<int> savePost(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateLikePost(Guid postId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if(post is not null)
            {
                post.Likes += 1;

                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUnlikePost(Guid postId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if (post is not null)
            {
                post.Likes -= 1;

                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAllPostsAsync()
        {
            var posts = await _dbContext.Posts.ToListAsync();
            _dbContext.RemoveRange(posts);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<List<string>> GetAllPosts()
        {
            var posts =  _dbContext.Posts.ToList();
            return posts.Select(post => post.PostId.ToString()).ToList();
        }
    }
}

