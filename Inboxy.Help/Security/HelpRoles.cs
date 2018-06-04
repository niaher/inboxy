namespace Inboxy.Help.Security
{
    using Inboxy.Infrastructure.Security;

    public class HelpRoles : RoleContainer
	{
		public static readonly SystemRole HelpReader = new SystemRole(nameof(HelpReader), true);
	}
}