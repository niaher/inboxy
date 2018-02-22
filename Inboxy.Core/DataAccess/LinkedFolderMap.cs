namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class LinkedFolderMap : IEntityTypeConfiguration<LinkedFolder>
	{
		public void Configure(EntityTypeBuilder<LinkedFolder> entity)
		{
			entity.ToTable("LinkedFolder");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.InboxId).HasColumnName("InboxId");
			entity.Property(t => t.Name).HasColumnName("Name").IsUnicode().HasMaxLength(LinkedFolder.NameMaxLength);
			entity.Property(t => t.NewItemsFolder).HasColumnName("NewItemsFolder").IsUnicode().HasMaxLength(LinkedFolder.NewItemsFolderMaxLength);
			entity.Property(t => t.ProcessedItemsFolder).HasColumnName("ProcessedItemsFolder").IsUnicode()
				.HasMaxLength(LinkedFolder.ProcessedItemsFolderMaxLength);

			entity.HasOne(t => t.Inbox)
				.WithMany(t => t.LinkedFolders)
				.HasForeignKey(t => t.InboxId);

			var emailsProperty = entity.Metadata.FindNavigation(nameof(LinkedFolder.Emails));
			emailsProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
			emailsProperty.SetField(LinkedFolder.EmailsFieldName);
		}
	}
}