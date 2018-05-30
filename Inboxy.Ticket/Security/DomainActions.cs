namespace Inboxy.Ticket.Security
{
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.Security.Ticket;

    public class DomainActions : ActionContainer
    {
        public static readonly SystemAction ListTicket = new SystemAction(nameof(ListTicket), TicketRole.AuthenticatedUser);
        public static readonly SystemAction CreateTicket = new SystemAction(nameof(CreateTicket), TicketRole.AuthenticatedUser);
        public static readonly SystemAction ViewTicket = new SystemAction(nameof(ViewTicket), TicketRole.AuthenticatedUser);
        public static readonly SystemAction GetInboxes = new SystemAction(nameof(GetInboxes), TicketRole.AuthenticatedUser);
        public static readonly SystemAction ReplyToTicket = new SystemAction(nameof(ReplyToTicket), TicketRole.AuthenticatedUser);
    }
}