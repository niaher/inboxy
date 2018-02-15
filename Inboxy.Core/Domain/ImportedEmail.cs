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
		protected ImportedEmail()
		{
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