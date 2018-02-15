namespace Inboxy.Core.Security.SomeThing
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;

	public class SomeThingAction : EntityAction<SomeThing, SomeThingRole>
	{
		public static SomeThingAction Edit = new SomeThingAction(nameof(Edit), SomeThingRole.Owner);
		public static SomeThingAction View = new SomeThingAction(nameof(View), SomeThingRole.Owner, SomeThingRole.Viewer);

		private SomeThingAction(string name, params SomeThingRole[] roles) : base(name, roles)
		{
		}
	}
}
