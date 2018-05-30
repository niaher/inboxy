// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local

namespace Inboxy.Ticket.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UiMetadataFramework.Basic.Output;

    public class Ticket
    {
        public const string CommentsFieldName = nameof(comments);
        private readonly List<TicketComment> comments = new List<TicketComment>();

        private Ticket()
        {
        }

        public Ticket(string subject, int? createdByUserId, RequesterUser requester, string details, Inbox inbox, int? linkedFolderId)
        {
            this.Subject = subject;
            this.CreatedOn = DateTime.UtcNow;
            this.CreatedByUserId = createdByUserId;
            this.LinkedFolderId = linkedFolderId;
            this.RequesterUser = requester;
            this.Inbox = inbox;

            this.comments.Add(TicketComment.InitialTicketComment(details));
        }

        /// <summary>
        /// Get id of user this ticket is assigned to
        /// </summary>
        public int AssigneeUserId { get; private set; }

        /// <summary>
        /// Get all comments 
        /// </summary>
        public IEnumerable<TicketComment> Comments => this.comments.AsReadOnly();

        /// <summary>
        /// Get the id of user who created this ticket - will be null if ticket create by system
        /// </summary>
        public int? CreatedByUserId { get; private set; }

        /// <summary>
        /// Get the tickter creation date
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// Get ticket Id
        /// </summary>
        public int Id { get; private set; }

        public Inbox Inbox { get; private set; }

        /// <summary>
        /// Get the inbox this ticket linked to
        /// </summary>
        public int InboxId { get; private set; }

        /// <summary>
        /// Get the linked folder id
        /// this can be null if this ticket is not directly linked to folder
        /// </summary>
        public int? LinkedFolderId { get; private set; }

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

        public TicketComment Details()
        {
            return this.Comments.FirstOrDefault(t => t.IsInitial);
        }

        public ActionList GetActions()
        {
            return new ActionList();
        }

        public string GetStatus()
        {
            return this.Status?.Name;
        }

        public IEnumerable<TicketComment> Replies()
        {
            return this.Comments?.Where(t => !t.IsInitial);
        }

        public void Reply(string comment, int userId)
        {
            this.comments.Add(new TicketComment(comment, null, userId));
        }
    }
}