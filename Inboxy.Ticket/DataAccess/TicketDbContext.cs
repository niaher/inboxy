namespace Inboxy.Ticket.DataAccess
{
    using Inboxy.Ticket.Domain;
    using Microsoft.EntityFrameworkCore;

    public class TicketDbContext : DbContext
    {
        public static readonly string Schema = "ticket";

        public TicketDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<InboxUser> InboxUsers { get; set; }
        public virtual DbSet<LinkedFolder> LinkedFolders { get; set; }
        public virtual DbSet<RequesterUser> RequesterUsers { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new TicketMap());
            builder.ApplyConfiguration(new TicketCommentMap());
            builder.ApplyConfiguration(new TicketStatusMap());
            builder.ApplyConfiguration(new RequesterUserMap());
            builder.ApplyConfiguration(new InboxMap());
            builder.ApplyConfiguration(new InboxUserMap());
            builder.ApplyConfiguration(new EmailMap());
            builder.ApplyConfiguration(new LinkedFolderMap());
        }
    }
}