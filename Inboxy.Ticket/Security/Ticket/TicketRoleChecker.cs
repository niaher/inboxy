namespace Inboxy.Ticket.Security.Ticket
{
    using CPermissions;
    using System;
    using System.Collections.Generic;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Domain;

    public class TicketRoleChecker: IRoleChecker<UserContext,TicketRole,Ticket>
    {
        public IEnumerable<TicketRole> GetRoles(UserContext user, Ticket context)
        {
            throw new NotImplementedException();
        }
    }
}
