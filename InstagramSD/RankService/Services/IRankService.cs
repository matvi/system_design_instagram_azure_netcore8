using System;
using Domain.Models;

namespace RankService.Services
{
	public interface IRankService
	{
        Task<bool> DeleteAll();
        Task<List<Guid>> GetListOfFriendsToSharePostByUserId(Guid userId);
        Task<int> InsertManualRank(List<Rank> rankList);
    }
}

