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


            var commentsProperty = builder.Metadata.FindNavigation(nameof(Ticket.Comments));
            commentsProperty.SetPropertyAccessMode(PropertyAccessMode.Field);
            commentsProperty.SetField(Ticket.CommentsFieldName);
        }
    }
}