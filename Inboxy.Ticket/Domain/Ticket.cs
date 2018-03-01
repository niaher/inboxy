// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Ticket.Domain
{
    using System;

    internal class Ticket
    {
        public Ticket(int? createdBy, Requester requester, int linkedFolderId)
        {
            this.CreatedOn = DateTime.UtcNow;
            this.CreatedBy = createdBy;
            this.Requester = requester;
            this.RequesterId = requester.Id;
            this.LinkedFolderId = linkedFolderId;
        }

        /// <summary>
        /// Get id of user this ticket is assigned to
        /// </summary>
        public int AssigneeUserId { get; private set; }

        /// <summary>
        /// Get the id of user who created this ticket - could be null if ticket create by system
        /// </summary>
        public int? CreatedBy { get; private set; }

        /// <summary>
        /// Get the tickter creation date
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// Get ticket Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Get the ticket requester, details about the reason why this ticket created.
        /// </summary>
        public virtual Requester Requester { get; private set; }

        /// <summary>
        /// Get ticket requester id
        /// </summary>
        public int RequesterId { get; private set; }

        /// <summary>
        /// Get current status of this ticket
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// Get the linked folder id
        /// </summary>
        public int LinkedFolderId { get; private set; }
    }
}