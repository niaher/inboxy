namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TicketMap: IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Ticket", TicketDbContext.Schema);
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();


            var usersProperty = builder.Metadata.FindNavigation(nameof(Ticket.Comments));
            usersProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
            usersProperty.SetField(Ticket.CommentsFieldName);

        }
    }
}