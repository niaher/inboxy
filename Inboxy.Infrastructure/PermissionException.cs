namespace Inboxy.Infrastructure
{
	using CPermissions;
	using Inboxy.Infrastructure.User;

	public class PermissionException : PermissionException<UserContext>
	{
		public PermissionException(string action, UserContext userContext) : base(new UserAction(action), userContext)
		{
		}
	}
}
