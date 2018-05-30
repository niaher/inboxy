namespace Inboxy.Ticket.Security.Inbox
{
    using CPermissions;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Domain;

    public class InboxPermissionManager : EntityPermissionManager<UserContext, InboxAction, InboxRole, Inbox>
    {
        public InboxPermissionManager(IRoleChecker<UserContext, InboxRole, Inbox> roleChecker) : base(roleChecker)
        {

        }
    }
}