namespace Inboxy.Core.Security.Inbox
{
	using Inboxy.Infrastructure.Security;

	public class InboxRole : Role
	{
		public static InboxRole Owner = new InboxRole(nameof(Owner));

		private InboxRole(string name) : base(name)
		{
		}
	}
}
