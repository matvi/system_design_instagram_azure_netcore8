namespace LikeService.Contracts
{
    public class LikePostRequest
    {

        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}

