namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;

    internal class TicketDbContext: DbContext
    {
        public static readonly string Schema = "ticket";
        public TicketDbContext(DbContextOptions options): base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<Requester> Requesters { get; set; }
        public virtual DbSet<RequesterUser> RequesterUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new TicketMap());
            builder.ApplyConfiguration(new TicketCommentMap());
            builder.ApplyConfiguration(new TicketStatusMap());
            builder.ApplyConfiguration(new RequesterMap());
            builder.ApplyConfiguration(new RequesterUserMap());

        }
    }
}