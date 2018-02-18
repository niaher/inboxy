// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Core.Domain
{
	using System;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents an email which was imported from the email server into 
	/// the application's database. The emails are actually imported by
	/// Inboxy.EmailReader and this assembly simply reads them.
	/// </summary>
	public class ImportedEmail : DomainEntityWithKeyInt32
	{
		internal const int FromMaxLength = 200;

		protected ImportedEmail()
		{
		}

		public ImportedEmail(Inbox inbox)
		{
			this.InboxId = inbox.Id;
			this.Inbox = inbox;
		}

		/// <summary>
		/// Gets email body.
		/// </summary>
		public string Body { get; private set; }

		/// <summary>
		/// Gets email address from which the email was received.
		/// </summary>
		public string From { get; private set; }

		/// <summary>
		/// Gets date when the email was imported into the system.
		/// </summary>
		public DateTime ImportedOn { get; private set; }

		public virtual Inbox Inbox { get; private set; }

		/// <summary>
		/// Gets inbox from which this email was imported.
		/// </summary>
		public int InboxId { get; private set; }

		/// <summary>
		/// Unique identifier for the email in the inbox.
		/// </summary>
		public string MessageId { get; private set; }

		/// <summary>
		/// Gets date when the email was actually received on the email server.
		/// </summary>
		public DateTime ReceivedOn { get; private set; }

		/// <summary>
		/// Gets email subject.
		/// </summary>
		public string Subject { get; private set; }
	}
}