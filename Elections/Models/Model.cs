using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Elections.Models
{
	public class Model : DbContext
	{
		public Model(DbContextOptions options) : base(options)
		{
			if (Database.GetService<IDatabaseCreator>() is SqlServerDatabaseCreator)
				Database.Migrate();
		}
	}
}
