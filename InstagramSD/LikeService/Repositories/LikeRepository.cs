using Domain.Models;
using LikeService.Data;
using Microsoft.EntityFrameworkCore;

namespace LikeService.Repositories
{
    public class LikeRepository : ILikeRepository
	{
        private readonly LikeDbContext _likeDbContext;

        public LikeRepository(LikeDbContext likeDbContext)
		{
            _likeDbContext = likeDbContext;
        }

        public async Task<int> AddLikeAsyc(Like like)
        {
            await _likeDbContext.AddAsync(like);
            return await _likeDbContext.SaveChangesAsync();
        }

        public async Task<Like?> UnlikeAsync(Guid userId, Guid postId)
        {
            var like = await _likeDbContext.Likes.FirstOrDefaultAsync(
                f => f.UserId == userId &&
                f.PostId == postId);

            if(like is null)
            {
                return null;
            }

            _likeDbContext.Likes.Remove(like);

            var result = (await _likeDbContext.SaveChangesAsync()) == 1;

            return result ? like : null;
        }
    }
}

