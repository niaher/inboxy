namespace Inboxy.Ticket.Security
{
    using Inboxy.Infrastructure.Security;

    public class DomainActions : ActionContainer
    {
        public static readonly SystemAction CreateTicket = new SystemAction(nameof(CreateTicket), TicketRole.AuthenticatedUser);
    }
}