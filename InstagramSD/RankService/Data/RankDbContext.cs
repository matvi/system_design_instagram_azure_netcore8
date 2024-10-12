using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace RankService.Data
{
	public class RankDbContext : DbContext
	{
		public RankDbContext(DbContextOptions<RankDbContext> dbContextOptions) : base(dbContextOptions)
		{}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Rank>()
				.ToContainer("Rank")
				.HasPartitionKey(r => r.UserId)
				.Property(u => u.Friends)
                .HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
						.Select(Guid.Parse)
						.ToList()
				);

		}

        public DbSet<Rank> Ranks { get; set; }
    }
}

