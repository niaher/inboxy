// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Core.Domain
{
	using System;
	using System.Collections.Generic;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents an email inbox.
	/// </summary>
	public class Inbox : DomainEntityWithKeyInt32
	{
		internal const int NameMaxLength = 200;
		internal const int EmailMaxLength = 200;
		internal const int ProcessedItemsFolderMaxLength = 200;
		internal const int NewItemsFolderMaxLength = 200;
		public const string EmailsFieldName = nameof(emails);
		public const string UsersFieldName = nameof(users);
		private readonly List<ImportedEmail> emails = new List<ImportedEmail>();
		private readonly List<InboxUser> users = new List<InboxUser>();

		protected Inbox()
		{
		}

		public Inbox(string email, string name, string newItemsFolder, string processedItemsFolder)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Email = email;
			this.Name = name;
			this.NewItemsFolder = newItemsFolder;
			this.ProcessedItemsFolder = processedItemsFolder;
		}

		public DateTime CreatedOn { get; private set; }
		public string Email { get; private set; }
		public IEnumerable<ImportedEmail> Emails => this.emails.AsReadOnly();
		public string Name { get; private set; }
		public string NewItemsFolder { get; private set; }
		public string ProcessedItemsFolder { get; private set; }
		public IEnumerable<InboxUser> Users => this.users.AsReadOnly();

		public void ChangeEmail(string email)
		{
			this.Email = email.CleanAndEnforceFormat(EmailMaxLength, nameof(this.Email));
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

		public InboxUser AddUser(RegisteredUser user)
		{
			this.users.Add(new InboxUser(this, user));
		}
	}
}