namespace Inboxy.EmailReader.DataAccess
{
	using Inboxy.EmailReader.Domain;
	using Microsoft.EntityFrameworkCore;

	public class EmailReaderDbContext : DbContext
	{
		public EmailReaderDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<Email> Emails { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new EmailMap());
		}
	}
}
