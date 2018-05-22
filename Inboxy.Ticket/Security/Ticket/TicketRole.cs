namespace Inboxy.Ticket.Security.Ticket
{
    using Inboxy.Infrastructure.Security;

    public class TicketRole: Role
    {
        public static readonly SystemRole AuthenticatedUser = new SystemRole(nameof(AuthenticatedUser),true);

        public TicketRole(string name) : base(name)
        {
        }
    }
}