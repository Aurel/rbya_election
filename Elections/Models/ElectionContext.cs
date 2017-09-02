using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Elections.Models
{
	public class ElectionContext : DbContext
	{
		public ElectionContext(DbContextOptions options) : base(options)
		{
			if (Database.GetService<IDatabaseCreator>() is SqlServerDatabaseCreator)
				Database.Migrate();
		}

		public DbSet<Candidate> Candidates { get; set; }
		public DbSet<Voter> Voters { get; set; }
		public DbSet<Vote> Votes { get; set; }
	}
}
