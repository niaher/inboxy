namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class ImportedEmailMap : IEntityTypeConfiguration<ImportedEmail>
	{
		public void Configure(EntityTypeBuilder<ImportedEmail> entity)
		{
			entity.ToTable("Email");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.MessageId).HasColumnName("MessageId").IsUnicode(false);
			entity.Property(t => t.Subject).HasColumnName("Subject").IsUnicode();
			entity.Property(t => t.Body).HasColumnName("Body").IsUnicode();
			entity.Property(t => t.From).HasColumnName("From").IsUnicode(false).HasMaxLength(ImportedEmail.FromMaxLength);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.ImportedOn).HasColumnName("ImportedOn");
			entity.Property(t => t.ReceivedOn).HasColumnName("ReceivedOn");
			entity.Property(t => t.LinkedFolderId).HasColumnName("LinkedFolderId");
			entity.Property(t => t.BodyType).HasColumnName("BodyType");

			entity.HasOne(t => t.LinkedFolder)
				.WithMany(t => t.Emails)
				.HasForeignKey(t => t.LinkedFolderId);
		}
	}
}