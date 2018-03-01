namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal struct RequesterMap : IEntityTypeConfiguration<Requester>
    {
        public void Configure(EntityTypeBuilder<Requester> builder)
        {
            builder.ToTable("Requester", TicketDbContext.Schema);
            builder.HasKey(t => t.Id);
        }
    }
}