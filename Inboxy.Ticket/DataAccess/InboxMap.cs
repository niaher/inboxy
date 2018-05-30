namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InboxMap: IEntityTypeConfiguration<Inbox>
    {
        public void Configure(EntityTypeBuilder<Inbox> builder)
        {
            builder.ToTable("Inbox", "ticket");
            builder.HasKey(t => t.Id);
            builder.HasMany(t => t.Users).WithOne().HasForeignKey(t => t.InboxId);
        }
    }
}