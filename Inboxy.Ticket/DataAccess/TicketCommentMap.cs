namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TicketCommentMap: IEntityTypeConfiguration<TicketComment>
    {
        public void Configure(EntityTypeBuilder<TicketComment> builder)
        {
            builder.ToTable("TicketComment", TicketDbContext.Schema);
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();
            builder.Property(t => t.TicketId).HasColumnName("TicketId");

            builder.HasOne(t => t.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(t => t.TicketId);
        }
    }
}