namespace Inboxy.Help.Security
{
    using System.Collections.Generic;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;

    public class HelpRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return HelpRoles.HelpReader;
			}
		}
	}
}