using System;
namespace Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
    }
}

