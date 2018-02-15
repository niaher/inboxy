namespace Inboxy.Core.Security
{
	using Inboxy.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction UseTools = new SystemAction(nameof(UseTools), CoreRoles.ToolUser);
		public static readonly SystemAction ViewFiles = new SystemAction(nameof(ViewFiles), CoreRoles.ToolUser);
	}
}
