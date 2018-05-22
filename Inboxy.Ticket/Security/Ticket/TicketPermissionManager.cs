namespace Inboxy.Ticket.Security.Ticket
{
    using CPermissions;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Domain;

    public class TicketPermissionManager: EntityPermissionManager<UserContext,TicketAction,TicketRole,Ticket>
    {
        public TicketPermissionManager(IRoleChecker<UserContext, TicketRole, Ticket> roleChecker) : base(roleChecker)
        {
        }
    }
}