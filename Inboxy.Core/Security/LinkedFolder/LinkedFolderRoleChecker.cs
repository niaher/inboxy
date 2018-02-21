namespace Inboxy.Core.Security.LinkedFolder
{
	using System.Collections.Generic;
	using System.Linq;
	using CPermissions;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.User;

	public class LinkedFolderRoleChecker : IRoleChecker<UserContext, LinkedFolderRole, LinkedFolder>
	{
		public IEnumerable<LinkedFolderRole> GetRoles(UserContext user, LinkedFolder context)
		{
			if (user.User.InboxIds.Contains(context.InboxId))
			{
				yield return LinkedFolderRole.Manager;
			}
		}
	}
}