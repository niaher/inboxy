namespace Inboxy.Ticket.Security.Inbox
{
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.Domain;

    public class InboxAction : EntityAction<Inbox, InboxRole>
    {
        public static readonly InboxAction ManageTickets = new InboxAction(nameof(ManageTickets),InboxRole.Administrator, InboxRole.Helpdesk);

        public InboxAction(string name, params InboxRole[] allowedRole) : base(name, allowedRole)
        {
        }
    }
}