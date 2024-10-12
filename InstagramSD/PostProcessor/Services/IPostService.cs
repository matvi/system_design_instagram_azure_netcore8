namespace PostProcessor.Services
{
    public interface IPostService
	{
        Task<List<Guid>> GetPostsListByUserIdAsync(Guid userId);
    }
}

