namespace Inboxy.Ticket.Security.Ticket
{
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.Domain;

    public class TicketAction : EntityAction<Ticket, TicketRole>
    {
        public static readonly TicketAction ViewTicket =  new TicketAction(nameof(ViewTicket));
        public TicketAction(string name, params TicketRole[] allowedRole) : base(name, allowedRole)
        {
        }
    }
}