namespace Inboxy.Core.Security.SomeThing
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class SomeThingPermissionManager : EntityPermissionManager<UserContext, SomeThingAction, SomeThingRole, SomeThing>
	{
		public SomeThingPermissionManager() : base(new SomeThingRoleChecker())
		{
		}
	}
}
