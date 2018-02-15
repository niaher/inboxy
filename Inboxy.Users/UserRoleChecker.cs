namespace Inboxy.Users
{
	using System.Collections.Generic;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using Inboxy.Users.Security;

	public class UserRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			yield return userData != null
				? UserManagementRoles.AuthenticatedUser
				: UserManagementRoles.UnauthenticatedUser;
		}
	}
}
