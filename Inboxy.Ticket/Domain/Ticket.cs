// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Ticket.Domain
{
    using System;
    using System.Collections.Generic;

    public class Ticket
    {
        public const string CommentsFieldName = nameof(comments);

        private readonly List<TicketComment> comments = new List<TicketComment>();

        public Ticket(string subject, int? createdBy, RequesterUser requester, TicketComment comment, int linkedFolderId)
        {
            this.Subject = subject;
            this.CreatedOn = DateTime.UtcNow;
            this.CreatedBy = createdBy;
            this.LinkedFolderId = linkedFolderId;
            this.RequesterUser = requester;
            this.comments.Add(comment);
        }

        /// <summary>
        /// Get id of user this ticket is assigned to
        /// </summary>
        public int AssigneeUserId { get; private set; }

        public IEnumerable<TicketComment> Comments => this.comments.AsReadOnly();

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
        /// Get the linked folder id
        /// </summary>
        public int LinkedFolderId { get; private set; }

        /// <summary>
        /// Get the ticket priority
        /// </summary>
        public TicketPriority Priority { get; private set; }

        /// <summary>
        /// Get the ticket requester, details about the reason why this ticket created.
        /// </summary>
        public RequesterUser RequesterUser { get; private set; }

        /// <summary>
        /// Get ticket requester id
        /// </summary>
        public int RequesterUserId { get; private set; }

        /// <summary>
        /// Get current status
        /// </summary>
        public virtual TicketStatus Status { get; private set; }

        /// <summary>
        /// Get current statu id 
        /// </summary>
        public int StatusId { get; private set; }

        /// <summary>
        /// Get ticket subject
        /// </summary>
        public string Subject { get; private set; }

        public TicketType Type { get; private set; }
    }
}