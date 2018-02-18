namespace Inboxy.Core.Domain
{
	public class InboxUser
	{
		protected InboxUser()
		{
		}

		public InboxUser(Inbox inbox, RegisteredUser user)
		{
			this.Inbox = inbox;
			this.InboxId = inbox.Id;
			this.UserId = user.Id;
		}

		public virtual Inbox Inbox { get; private set; }
		public int InboxId { get; private set; }
		public int UserId { get; private set; }
	}
}