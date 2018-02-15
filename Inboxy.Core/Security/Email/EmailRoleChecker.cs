namespace Inboxy.Core.Security.Email
{
	using System.Collections.Generic;
	using CPermissions;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.User;

	public class EmailRoleChecker : IRoleChecker<UserContext, EmailRole, ImportedEmail>
	{
		public IEnumerable<EmailRole> GetRoles(UserContext user, ImportedEmail context)
		{
			if (user.HasRole(CoreRoles.ToolUser))
			{
				yield return EmailRole.Viewer;
			}
		}
	}
}