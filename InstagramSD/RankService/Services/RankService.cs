using System;
using Domain.Models;
using RankService.Repositories;

namespace RankService.Services
{
	public class RankService : IRankService
	{
        private readonly IRankRepository _rankRepository;

        public RankService(IRankRepository rankRepository)
		{
            this._rankRepository = rankRepository;
        }

        public async Task<bool> DeleteAll()
        {
            return await _rankRepository.DeleteAllAsync();
        }

        public async Task<List<Guid>> GetListOfFriendsToSharePostByUserId(Guid userId)
        {
            var rank = await _rankRepository.GetRankByUserId(userId);
            if(rank is null)
            {
                return null;
            }

            return rank.Friends.ToList();
        }

        public async Task<int> InsertManualRank(List<Rank> rankList)
        {
            return await _rankRepository.AddRangeAsync(rankList);
        }
    }
}

