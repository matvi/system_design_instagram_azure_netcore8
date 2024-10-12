using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace PostService.Data
{
    public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{
		}

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .ToContainer("Post")
                .HasPartitionKey(e => e.PostId);
        }

    }
}

