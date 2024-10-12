using System.Text.Json;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Common.ServiceBus;
using Domain.Dtos;
using Domain.Enums;
using Domain.EventMessages;
using Domain.Models;
using Microsoft.Extensions.Options;
using PostService.Models;
using PostService.Repositories;
using PostService.Settings;

namespace PostService.Services
{
    public class PostInstagramService : IPostService
	{
        private readonly BlobSettings _blobSettings;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IPostRepository _postRepository;
        private readonly IEventBus _eventBus;

        public PostInstagramService(
            IOptions<BlobSettings> blobSettings,
            BlobServiceClient blobServiceClient,
            IPostRepository postRepository,
            IEventBus eventBus)
        {
            _blobSettings = blobSettings.Value;

            // Create a BlobServiceClient object 
            _blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
            _postRepository = postRepository;
            _eventBus = eventBus;
        }

        public async Task<PostResult> CreatePost(UploadRequest uploadRequest)
        {
            var saveBlobResult = await SaveImageBlobStorage(uploadRequest.file);
            if (!saveBlobResult.IsSuccess)
            {
                return new PostResult
                {
                    IsSuccess = false
                };
            }

            Post post = new()
            {
                ImageUrl = saveBlobResult.ImageUrl,
                ImageFileName = saveBlobResult.ImageFileName,
                PostText = uploadRequest.PostText,
                PostTitle = uploadRequest.PostTitle,
                Likes = 0,
                User = getUserByUserId(uploadRequest.UserId)

            };

            var savePostInDbResult = await _postRepository.savePost(post);

            if(savePostInDbResult == 1)
            {
                var postCreated = CreatePostCreatedEventFromPost(post); // use factory pattern
                await _eventBus.PublishMessage(postCreated);
            }

            PostResult postResult = new()
            {
                ImageUrl = saveBlobResult.ImageUrlWithSasToken,
                IsSuccess = (savePostInDbResult == 1),
                PostId = post.PostId
            };

            return postResult;
            
        }

        private User getUserByUserId(Guid userId)
        {
            //this is dummy data.
            //for real case scenario get the information from Identity Token
            var dummyUsersAsJsonString = "[ { \"UserId\": \"1fa85f64-5717-4562-b3fc-2c963f66afa6\", \"UserName\": \"Alpha\", \"UserProfileImageUrl\": \"https://davidmata.blob.core.windows.net/userprofile/user1.png?sp=r&st=2024-04-13T19:23:15Z&se=2026-04-02T03:23:15Z&spr=https&sv=2022-11-02&sr=b&sig=dm0%2F3S7P9ouXX5Au2%2FrzkyU40EXPyiyTHwwQRripMz0%3D\" }, { \"UserId\": \"2fa85f64-5717-4562-b3fc-2c963f66afa6\", \"UserName\": \"Beta\", \"UserProfileImageUrl\": \"https://davidmata.blob.core.windows.net/userprofile/user2.png?sp=r&st=2024-04-13T19:24:31Z&se=2025-06-20T03:24:31Z&spr=https&sv=2022-11-02&sr=b&sig=3MmlaY28%2FEaWnntMN8xlGz1yClnb%2B9r5Oix%2F8J%2FIZc0%3D\" }, { \"UserId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\", \"UserName\": \"Charles\", \"UserProfileImageUrl\": \"https://davidmata.blob.core.windows.net/userprofile/user3.png?sp=r&st=2024-04-13T19:25:07Z&se=2025-06-20T03:25:07Z&spr=https&sv=2022-11-02&sr=b&sig=Yrr8v%2BOkSFKfLh%2F%2Fx9nx%2FP2Qye3PX31l3nCGbtoy5Hk%3D\" }, { \"UserId\": \"4fa85f64-5717-4562-b3fc-2c963f66afa6\", \"UserName\": \"Delta\", \"UserProfileImageUrl\": \"https://davidmata.blob.core.windows.net/userprofile/user4.png?sp=r&st=2024-04-13T19:25:43Z&se=2025-02-12T03:25:43Z&spr=https&sv=2022-11-02&sr=b&sig=gUlzV9xCifkDt6%2FBydcPcZl12%2BN%2BTw0jrY3xDK2Ua%2FQ%3D\" } ]";
            var users = JsonSerializer.Deserialize<List<User>>(dummyUsersAsJsonString);

            var user = users.FirstOrDefault(u => u.UserId == userId);

            return user;
        }

        private PostCreated CreatePostCreatedEventFromPost(Post post)
        {
            return new PostCreated()
            {
                PostId = post.PostId,
                PostText = post.PostText,
                PostTitle = post.PostTitle,
                ImageFileName = post.ImageFileName,
                ImageUrl = post.ImageUrl,
                User = new User{
                    UserProfileImageUrl = post.User.UserProfileImageUrl,
                    UserId = post.User.UserId,
                    UserName = post.User.UserName
                },
                EventType = EventType.PostCreated.ToString()
            };
        }

        private async Task<SaveBlobResult> SaveImageBlobStorage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new SaveBlobResult
                {
                    IsSuccess = false
                };
            }

            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);
            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(blobName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            var sasToken = GenerateSasToken(blobName, BlobContainerSasPermissions.Read);

            // Return the URL with the SAS token
            string imageUrlWithSasToken = $"{blobClient.Uri}?{sasToken}";

            return new SaveBlobResult
            {
                ImageUrl = blobClient.Uri.ToString(),
                ImageUrlWithSasToken = imageUrlWithSasToken,
                ImageFileName = blobName
            };
        }

        public async Task<List<PostDto>> GetPostInformationByPostIds(List<Guid> postIds)
        {
            var posts = await _postRepository.GetPostInformationByPostIds(postIds);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);
            var postDto = posts.Select(post => new PostDto
            {
                PostId = post.PostId,
                ImageName = post.ImageFileName,
                PostText = post.PostText,
                User = post.User,
                Likes = post.Likes,
                ImageUrl = $"{post.ImageUrl}?{GenerateSasToken(post.ImageFileName, BlobContainerSasPermissions.Read)}"
            });

            return postDto.ToList();
        }

        private string GenerateSasToken(string fileName, BlobContainerSasPermissions permissions)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobSettings.ContainerName, // Container name is the second segment in the URI
                BlobName = fileName,          // Blob name is the third segment in the URI
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),
            };

            sasBuilder.SetPermissions(permissions);

            var storageSharedCredentials = new StorageSharedKeyCredential(_blobSettings.AccountName, _blobSettings.AccountKey);
            BlobSasQueryParameters sasToken = sasBuilder.ToSasQueryParameters(storageSharedCredentials);
            return sasToken.ToString();
        }

        public async Task<bool> DeletePost(Guid id)
        {
            return await _postRepository.DeletePost(id);
        }

        public async Task<int> DeleteAllPost()
        {
            return await _postRepository.DeleteAllPostsAsync();
        }

        public async Task<List<string>> GetAllPost()
        {
            return await _postRepository.GetAllPosts();
        }
    }
}

