// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Core.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents a folder inside an email inbox which will be monitored for incoming emails.
	/// </summary>
	public class LinkedFolder : DomainEntityWithKeyInt32
	{
		internal const int NameMaxLength = 200;
		internal const int ProcessedItemsFolderMaxLength = 200;
		internal const int NewItemsFolderMaxLength = 200;
		public const string EmailsFieldName = nameof(emails);
		public const string UsersFieldName = nameof(users);
		private readonly List<ImportedEmail> emails = new List<ImportedEmail>();
		private readonly List<InboxUser> users = new List<InboxUser>();

		protected LinkedFolder()
		{
		}

		public LinkedFolder(Inbox inbox, string name, string newItemsFolder, string processedItemsFolder)
		{
			this.InboxId = inbox.Id;
			this.Inbox = inbox;
			this.CreatedOn = DateTime.UtcNow;
			this.Name = name;
			this.NewItemsFolder = newItemsFolder;
			this.ProcessedItemsFolder = processedItemsFolder;
		}

		public DateTime CreatedOn { get; private set; }
		public IEnumerable<ImportedEmail> Emails => this.emails.AsReadOnly();
		public virtual Inbox Inbox { get; private set; }
		public int InboxId { get; private set; }
		public string Name { get; private set; }
		public string NewItemsFolder { get; private set; }
		public string ProcessedItemsFolder { get; private set; }
		public IEnumerable<InboxUser> Users => this.users.AsReadOnly();

		public InboxUser AddUser(RegisteredUser user)
		{
			var inboxUser = this.users.SingleOrDefault(t => t.UserId == user.Id);

			if (inboxUser == null)
			{
				inboxUser = new InboxUser(this, user);
				this.users.Add(inboxUser);
			}

			return inboxUser;
		}

		public void ChangeName(string name)
		{
			this.Name = name.CleanAndEnforceFormat(NameMaxLength, nameof(this.Name));
		}

		public void ChangeNewItemsFolder(string folderName)
		{
			this.NewItemsFolder = folderName.CleanAndEnforceFormat(NewItemsFolderMaxLength, nameof(this.NewItemsFolder));
		}

		public void ChangeProcessedItemsFolder(string folderName)
		{
			this.ProcessedItemsFolder = folderName.CleanAndEnforceFormat(ProcessedItemsFolderMaxLength, nameof(this.ProcessedItemsFolder));
		}

		public void RemoveUser(RegisteredUser user)
		{
			var toRemove = this.users.Where(t => t.UserId == user.Id).ToList();

			if (this.users.Count > 0 &&
				toRemove.Count > 0 &&
				this.users.Count - toRemove.Count == 0)
			{
				throw new BusinessException("Cannot remove user, because it would result with an inbox which has no users.");
			}

			this.users.RemoveRange(toRemove);
		}
	}
}