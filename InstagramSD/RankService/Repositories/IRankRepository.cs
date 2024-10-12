using System;
using Domain.Models;

namespace RankService.Repositories
{
	public interface IRankRepository
	{
        Task<int> AddRangeAsync(List<Rank> ranks);
        Task<bool> DeleteAllAsync();
        Task<Rank> GetRankByUserId(Guid userId);
    }
}

