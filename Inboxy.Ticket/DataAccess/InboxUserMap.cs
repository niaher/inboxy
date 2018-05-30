namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InboxUserMap: IEntityTypeConfiguration<InboxUser>
    {
        public void Configure(EntityTypeBuilder<InboxUser> builder)
        {
            builder.ToTable("InboxUser", "ticket");
            builder.HasKey(t => new { t.InboxId, t.UserId });
        }
    }
}