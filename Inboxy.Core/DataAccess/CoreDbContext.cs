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
		public virtual DbSet<Inbox> Inboxes { get; set; }
		public virtual DbSet<InboxUser> InboxUsers { get; set; }
		public virtual DbSet<RegisteredUser> RegisteredUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new ImportedEmailMap());
			builder.ApplyConfiguration(new InboxMap());
			builder.ApplyConfiguration(new InboxUserMap());
			builder.ApplyConfiguration(new RegisteredUserMap());
		}
	}
}