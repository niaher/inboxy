namespace Inboxy.Core.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents an email account.
	/// </summary>
	public class Inbox : DomainEntityWithKeyInt32
	{
		public const string UsersFieldName = nameof(users);
		internal const string LinkedFoldersFieldName = nameof(linkedFolders);
		internal const int NameMaxLength = 200;
		internal const int EmailMaxLength = 200;
		private readonly List<LinkedFolder> linkedFolders = new List<LinkedFolder>();
		private readonly List<InboxUser> users = new List<InboxUser>();

		protected Inbox()
		{
		}

		public Inbox(string email, string name, params RegisteredUser[] user)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.ChangeEmail(email);
			this.ChangeName(name);
			this.AddUsers(user);
		}

		public DateTime CreatedOn { get; private set; }

		public string Email { get; private set; }
		public IEnumerable<LinkedFolder> LinkedFolders => this.linkedFolders.AsReadOnly();
		public string Name { get; private set; }

		public IEnumerable<InboxUser> Users => this.users.AsReadOnly();

		public LinkedFolder AddFolder(string name, string newItemsFolder, string processedItemsFolder)
		{
			var folder = new LinkedFolder(
				this,
				name,
				newItemsFolder,
				processedItemsFolder);

			this.linkedFolders.Add(folder);

			return folder;
		}

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

		public void AddUsers(params RegisteredUser[] usersToAdd)
		{
			foreach (var user in usersToAdd)
			{
				this.AddUser(user);
			}
		}

		public void ChangeEmail(string email)
		{
			this.Email = email.CleanAndEnforceFormat(EmailMaxLength, nameof(this.Email));
		}

		public void ChangeName(string name)
		{
			this.Name = name.CleanAndEnforceFormat(NameMaxLength, nameof(this.Name));
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