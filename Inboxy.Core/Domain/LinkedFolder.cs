// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Core.Domain
{
	using System;
	using System.Collections.Generic;
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

		private readonly List<ImportedEmail> emails = new List<ImportedEmail>();

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
	}
}