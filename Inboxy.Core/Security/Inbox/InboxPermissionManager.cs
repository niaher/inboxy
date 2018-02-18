namespace Inboxy.Core.Security.Inbox
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class InboxPermissionManager : EntityPermissionManager<UserContext, InboxAction, InboxRole, Inbox>
	{
		public InboxPermissionManager() : base(new InboxRoleChecker())
		{
		}
	}
}
