namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LinkedFolderMap: IEntityTypeConfiguration<LinkedFolder>
    {
        public void Configure(EntityTypeBuilder<LinkedFolder> builder)
        {
            builder.ToTable("LinkedFolder");
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Inbox).WithMany().HasForeignKey(t => t.InboxId);
        }
    }
}