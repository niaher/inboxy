namespace Inboxy.Ticket.Security.Inbox
{
    using System.Collections.Generic;
    using System.Linq;
    using CPermissions;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Domain;

    public class InboxRoleChecker : IRoleChecker<UserContext, InboxRole, Inbox>
    {
        public IEnumerable<InboxRole> GetRoles(UserContext user, Inbox context)
        {
            if (user.User.InboxIds.Contains(context.Id))
            {
                yield return InboxRole.Administrator;
            }
        }
    }
}