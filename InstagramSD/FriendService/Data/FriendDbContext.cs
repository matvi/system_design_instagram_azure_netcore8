using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendService.Data
{
    public class FriendDbContext : DbContext
	{
		public FriendDbContext(DbContextOptions<FriendDbContext> options):base(options)
		{
		}

        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FollowConfiguration());
            modelBuilder.Entity<Follow>()
                .ToContainer("Follow")
                .HasPartitionKey(e => e.FollowType);
        }
    }
}

