namespace Inboxy.Ticket.Security.Inbox
{
    using Inboxy.Infrastructure.Security;

    public class InboxRole : Role
    {
        public static readonly InboxRole Helpdesk = new InboxRole(nameof(Helpdesk));

        public InboxRole(string name) : base(name)
        {
        }
    }
}