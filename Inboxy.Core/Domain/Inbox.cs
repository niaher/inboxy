namespace Inboxy.Core.Domain
{
	using System;
	using System.Collections.Generic;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents an email account.
	/// </summary>
	public class Inbox : DomainEntityWithKeyInt32
	{
		internal const string LinkedFoldersFieldName = nameof(linkedFolders);
		internal const int NameMaxLength = 200;
		internal const int EmailMaxLength = 200;
		private readonly List<LinkedFolder> linkedFolders = new List<LinkedFolder>();

		protected Inbox()
		{
		}

		public Inbox(string email, string name)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Email = email;
			this.Name = name;
		}

		public DateTime CreatedOn { get; set; }

		public string Email { get; set; }
		public IEnumerable<LinkedFolder> LinkedFolders => this.linkedFolders.AsReadOnly();
		public string Name { get; set; }

		public void ChangeEmail(string email)
		{
			this.Email = email.CleanAndEnforceFormat(EmailMaxLength, nameof(this.Email));
		}

		public void ChangeName(string name)
		{
			this.Name = name.CleanAndEnforceFormat(NameMaxLength, nameof(this.Name));
		}
	}
}