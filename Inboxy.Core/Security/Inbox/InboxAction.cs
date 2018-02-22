namespace Inboxy.Core.Security.Inbox
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;

	public class InboxAction : EntityAction<Inbox, InboxRole>
	{
		public static InboxAction Manage = new InboxAction(nameof(Manage), InboxRole.Owner);

		private InboxAction(string name, params InboxRole[] roles) : base(name, roles)
		{
		}
	}
}