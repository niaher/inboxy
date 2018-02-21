namespace Inboxy.Core.Domain
{
	public class InboxUser
	{
		protected InboxUser()
		{
		}

		internal InboxUser(LinkedFolder linkedFolder, RegisteredUser user)
		{
			this.LinkedFolder = linkedFolder;
			this.InboxId = linkedFolder.Id;
			this.User = user;
			this.UserId = user.Id;
		}

		public virtual LinkedFolder LinkedFolder { get; private set; }
		public int InboxId { get; private set; }
		public virtual RegisteredUser User { get; private set; }
		public int UserId { get; private set; }
	}
}