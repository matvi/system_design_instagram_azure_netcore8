using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LikeService.Data
{
	public class LikeDbContext : DbContext
	{
		public LikeDbContext(DbContextOptions<LikeDbContext> options):base(options)
		{
		}

        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .ToContainer("Like")
                .HasPartitionKey(e => e.PostId);
        }
    }
}

