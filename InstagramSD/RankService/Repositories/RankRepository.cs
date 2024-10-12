using Domain.Models;
using Microsoft.EntityFrameworkCore;
using RankService.Data;

namespace RankService.Repositories
{
    public class RankRepository : IRankRepository
	{
        private readonly RankDbContext _rankDbContext;

        public RankRepository(RankDbContext rankDbContext)
		{
            _rankDbContext = rankDbContext;
        }

        public async Task<Rank> GetRankByUserId(Guid userId)
        {
            var rank = await _rankDbContext.Ranks.FirstOrDefaultAsync(u => u.UserId == userId);
            return rank;
        }

        public async Task<int> AddRangeAsync(List<Rank> ranks)
        {
            await _rankDbContext.AddRangeAsync(ranks);
            return await _rankDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAllAsync()
        {
            var ranks = await _rankDbContext.Ranks.ToListAsync();
            _rankDbContext.Ranks.RemoveRange(ranks);
            var result = await _rankDbContext.SaveChangesAsync();
            return result == ranks.Count;
        }
    }
}

