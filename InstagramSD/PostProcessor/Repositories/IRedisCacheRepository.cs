using System;
namespace PostProcessor.Repositories
{
	public interface IRedisCacheRepository
	{
        Task updatePostsIdsInUserList(Guid postId, List<Guid> listOfUsers);
        Task<T> GetCasheData<T>(string key);
        Task<bool> RemoveData(string key);
        Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime);
        Task<List<T>> GetCasheDataList<T>(string key);
        Task<List<string>> GetCasheDataListString(string key);
    }
}

