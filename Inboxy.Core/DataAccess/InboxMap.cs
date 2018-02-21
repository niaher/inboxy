namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class InboxMap : IEntityTypeConfiguration<Inbox>
	{
		public void Configure(EntityTypeBuilder<Inbox> entity)
		{
			entity.ToTable("Inbox");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.Email).HasColumnName("Email");
			entity.Property(t => t.Name).HasColumnName("Name");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");

			var linkedFoldersProperty = entity.Metadata.FindNavigation(nameof(Inbox.LinkedFolders));
			linkedFoldersProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
			linkedFoldersProperty.SetField(Inbox.LinkedFoldersFieldName);
		}
	}
}