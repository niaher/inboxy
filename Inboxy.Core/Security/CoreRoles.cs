namespace Inboxy.Core.Security
{
	using Inboxy.Infrastructure.Security;

	public class CoreRoles : RoleContainer
	{
		public static readonly SystemRole ToolUser = new SystemRole(nameof(ToolUser));
		public static readonly SystemRole Anyone = new SystemRole(nameof(Anyone));
	}
}