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
			entity.Property(t => t.Email).HasColumnName("Email").IsUnicode(false).HasMaxLength(Inbox.EmailMaxLength);
			entity.Property(t => t.Name).HasColumnName("Name").IsUnicode().HasMaxLength(Inbox.NameMaxLength);
			entity.Property(t => t.NewItemsFolder).HasColumnName("NewItemsFolder").IsUnicode().HasMaxLength(Inbox.NewItemsFolderMaxLength);
			entity.Property(t => t.ProcessedItemsFolder).HasColumnName("ProcessedItemsFolder").IsUnicode().HasMaxLength(Inbox.ProcessedItemsFolderMaxLength);

			var emailsProperty = entity.Metadata.FindNavigation(nameof(Inbox.Emails));
			emailsProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
			emailsProperty.SetField(Inbox.EmailsFieldName);

			var usersProperty = entity.Metadata.FindNavigation(nameof(Inbox.Users));
			usersProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
			usersProperty.SetField(Inbox.UsersFieldName);
		}
	}
}