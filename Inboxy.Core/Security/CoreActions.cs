namespace Inboxy.Core.Security
{
	using Inboxy.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction UseTool = new SystemAction(nameof(UseTool), CoreRoles.ToolUser);
		public static readonly SystemAction ViewFiles = new SystemAction(nameof(ViewFiles), CoreRoles.ToolUser);
	}
}
