namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;

	public class CoreDbContext : DbContext
	{
		public CoreDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<ImportedEmail> ImportedEmails { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new ImportedEmailMap());
		}
	}
}