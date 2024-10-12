using Domain.Dtos;
using PostService.Models;

namespace PostService.Services
{
    public interface IPostService
    {
        Task<PostResult> CreatePost(UploadRequest uploadRequest);
        Task<int> DeleteAllPost();
        Task<bool> DeletePost(Guid id);
        Task<List<String>> GetAllPost();
        Task<List<PostDto>> GetPostInformationByPostIds(List<Guid> postIds);
    }
}