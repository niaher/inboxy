﻿namespace Inboxy.Core.Domain
{
	public class InboxUser
	{
		protected InboxUser()
		{
		}

		internal InboxUser(Inbox inbox, RegisteredUser user)
		{
			this.Inbox = inbox;
			this.InboxId = inbox.Id;
			this.User = user;
			this.UserId = user.Id;
		}

		public virtual Inbox Inbox { get; private set; }
		public int InboxId { get; private set; }
		public virtual RegisteredUser User { get; private set; }
		public int UserId { get; private set; }
	}
}