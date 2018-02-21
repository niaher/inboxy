namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class InboxUserMap : IEntityTypeConfiguration<InboxUser>
	{
		public void Configure(EntityTypeBuilder<InboxUser> entity)
		{
			entity.ToTable("InboxUser");
			entity.HasKey(t => new { InboxId = t.InboxId, UserId = t.UserId });
			entity.Property(t => t.InboxId).HasColumnName("InboxId");
			entity.Property(t => t.UserId).HasColumnName("UserId");

			entity.HasOne(t => t.LinkedFolder)
				.WithMany(t => t.Users)
				.HasForeignKey(t => t.InboxId);
		}
	}
}