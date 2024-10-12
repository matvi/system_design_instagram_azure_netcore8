using System;
namespace Domain.Models
{
	public class Rank
	{
		public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

		public IList<Guid> Friends { get; set; }
    }
}

