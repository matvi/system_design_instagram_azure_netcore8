using Domain.Models;

namespace PostService.Repositories
{
    public interface IPostRepository
    {
        Task<int> savePost(Post post);
        Task<List<Post>> GetPostInformationByPostIds(List<Guid> postIds);
        Task<bool> DeletePost(Guid id);
        Task UpdateLikePost(Guid postId);
        Task UpdateUnlikePost(Guid postId);
        Task<int> DeleteAllPostsAsync();
        Task<List<string>> GetAllPosts();
    }
}