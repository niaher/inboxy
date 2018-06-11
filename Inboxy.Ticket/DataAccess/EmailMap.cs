namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmailMap: IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(t => t.Id);
            builder.ToTable("Email");

            builder.HasOne(t => t.LinkedFolder).WithMany().HasForeignKey(t => t.LinkedFolderId);
        }
    }
}