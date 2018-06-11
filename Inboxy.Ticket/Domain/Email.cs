namespace Inboxy.Ticket.Domain
{
    using System;
    using Inboxy.Infrastructure.Domain;

    /// <summary>
    /// Represent an email that could be used to generate a ticket
    /// </summary>
    public class Email : DomainEntityWithKeyInt32
    {
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

        public virtual LinkedFolder LinkedFolder { get; private set; }

        /// <summary>
        /// This represent the id of the folder where this email extracted from
        /// it can be something like "inbox","important-emails" ... etc
        /// </summary>
        public int LinkedFolderId { get; private set; }

        /// <summary>
        /// Gets date when the email was actually received on the email server.
        /// </summary>
        public DateTime ReceivedOn { get; set; }

        /// <summary>
        /// Gets email subject.
        /// </summary>
        public string Subject { get; set; }
    }
}