namespace Inboxy.Ticket.Domain
{
    using Inboxy.Infrastructure.Domain;

    public class LinkedFolder : DomainEntityWithKeyInt32
    {
        public int InboxId { get; private set; }
        public Inbox Inbox { get; private set; }
        public string Name { get; private set; }
    }
}