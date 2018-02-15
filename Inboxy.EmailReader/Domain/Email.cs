// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Inboxy.EmailReader.Domain
{
	using System;
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents an email which was read of the email server.
	/// </summary>
	public class Email : DomainEntityWithKeyInt32
	{
		internal const int FromMaxLength = 200;

		protected Email()
		{
		}

		internal Email(string messageId, string from, string subject, string body, DateTime receivedOn)
		{
			this.MessageId = messageId;
			this.ImportedOn = DateTime.UtcNow;
			this.Subject = subject;
			this.From = from;
			this.Body = body;
			this.ReceivedOn = receivedOn;
		}

		/// <summary>
		/// Gets email body.
		/// </summary>
		public string Body { get; private set; }

		/// <summary>
		/// Unique identifier for the email message in the inbox.
		/// </summary>
		public string MessageId { get; private set; }

		/// <summary>
		/// Gets email address from which the email was received.
		/// </summary>
		public string From { get; private set; }

		/// <summary>
		/// Gets date when the email was imported into the system.
		/// </summary>
		public DateTime ImportedOn { get; private set; }

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