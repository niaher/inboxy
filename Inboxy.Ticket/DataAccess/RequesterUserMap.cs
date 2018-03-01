namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class RequesterUserMap: IEntityTypeConfiguration<RequesterUser>
    {
        public void Configure(EntityTypeBuilder<RequesterUser> builder)
        {
            builder.ToTable("RequesterUser", TicketDbContext.Schema);
            builder.HasKey(t => t.Id);
        }
    }
}