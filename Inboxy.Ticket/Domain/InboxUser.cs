namespace Inboxy.Ticket.Domain
{
    public class InboxUser
    {
        public int UserId { get; private set; }
        public int InboxId { get; private set; }

        public virtual Inbox Inbox { get; set; }
    }
}