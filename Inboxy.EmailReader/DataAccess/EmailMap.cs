namespace Inboxy.EmailReader.DataAccess
{
	using Inboxy.EmailReader.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	internal class EmailMap : IEntityTypeConfiguration<Email>
	{
		public void Configure(EntityTypeBuilder<Email> entity)
		{
			entity.ToTable("Email");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.MessageId).HasColumnName("MessageId").IsUnicode(false);
			entity.Property(t => t.Subject).HasColumnName("Subject").IsUnicode();
			entity.Property(t => t.Body).HasColumnName("Body").IsUnicode();
			entity.Property(t => t.From).HasColumnName("From").IsUnicode(false).HasMaxLength(Email.FromMaxLength);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.ImportedOn).HasColumnName("ImportedOn");
			entity.Property(t => t.ReceivedOn).HasColumnName("ReceivedOn");
		}
	}
}