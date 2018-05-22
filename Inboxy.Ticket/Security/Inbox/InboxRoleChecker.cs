namespace Inboxy.Ticket.Security.Inbox
{
    using System.Collections.Generic;
    using CPermissions;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Domain;
    public class InboxRoleChecker: IRoleChecker<UserContext,InboxRole,Inbox>
    {
        public IEnumerable<InboxRole> GetRoles(UserContext user, Inbox context)
        {
            throw new System.NotImplementedException();
        }
    }
}