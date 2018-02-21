namespace Inboxy.Core.Security.Inbox
{
	using System.Collections.Generic;
	using System.Linq;
	using CPermissions;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.User;

	public class InboxRoleChecker : IRoleChecker<UserContext, InboxRole, LinkedFolder>
	{
		public IEnumerable<InboxRole> GetRoles(UserContext user, LinkedFolder context)
		{
			if (user.User.InboxIds.Contains(context.Id))
			{
				yield return InboxRole.Owner;
			}
		}
	}
}