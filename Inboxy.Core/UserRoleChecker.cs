namespace Inboxy.Core
{
	using System.Collections.Generic;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class UserRoleChecker : IUserRoleChecker
	{
		/// <inheritdoc />
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return CoreRoles.ToolUser;
			}

			yield return CoreRoles.Anyone;
		}
	}
}
