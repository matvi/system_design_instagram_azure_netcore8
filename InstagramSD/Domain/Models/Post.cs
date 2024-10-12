namespace Domain.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageFileName { get; set; }
        public string? PostTitle { get; set; }
        public string? PostText { get; set; }
        public int? Likes { get; set; }
        public User User { get; set; } = new User();
    }
}

