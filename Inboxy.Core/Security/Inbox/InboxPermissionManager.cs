namespace Inboxy.Core.Security.Inbox
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class InboxPermissionManager : EntityPermissionManager<UserContext, InboxAction, InboxRole, LinkedFolder>
	{
		public InboxPermissionManager() : base(new InboxRoleChecker())
		{
		}
	}
}
