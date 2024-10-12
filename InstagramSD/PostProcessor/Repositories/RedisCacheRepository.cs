using System.Text.Json;
using Microsoft.Extensions.Options;
using PostProcessor.Settings;
using StackExchange.Redis;

namespace PostProcessor.Repositories
{
    public class RedisCacheRepository : IRedisCacheRepository
	{
        private readonly RedisSettings _redisSettings;
        private readonly IDatabase _db;

        public RedisCacheRepository(
            IOptions<RedisSettings> redisSettings,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _redisSettings = redisSettings.Value;
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetCasheData<T>(string key)
        {
            var data = await _db.StringGetAsync(key);
            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task<List<T>> GetCasheDataList<T>(string key)
        {
            var data = await _db.ListRangeAsync(key);
            if (data is null)
            {
                return default;
            }

            var result = data.Select(redisValue => JsonSerializer.Deserialize<T>(redisValue.ToString())).ToList();

            return result;
        }

        public async Task<List<string>> GetCasheDataListString(string key)
        {
            var data = await _db.ListRangeAsync(key);
            if (data is null)
            {
                return default;
            }

            var result = data.Select(redisValue => redisValue.ToString()).ToList();

            return result;
        }

        public async Task<bool> RemoveData(string key)
        {
            var isKeyExist = await _db.KeyExistsAsync(key);
            if (!isKeyExist)
            {
                return false;
            }
            return  await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiration = expirationTime.DateTime.Subtract(DateTime.Now);
            var result = await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiration);
            return result;
        }

        public async Task updatePostsIdsInUserList(Guid postId, List<Guid> listOfUsers)
		{
            // Prepare Lua script
            string script = @"
                for _, userId in ipairs(KEYS) do
                    redis.call('RPUSH', 'userId:' .. userId, ARGV[1])
                end
            ";

            // Execute Lua script atomically
            RedisResult result = await _db.ScriptEvaluateAsync(script, listOfUsers.Select(id => (RedisKey)$"userId:{id}").ToArray(), new RedisValue[] { postId.ToString() });

            Console.WriteLine("PostId added to Redis for each userId.");
        }
	}
}

