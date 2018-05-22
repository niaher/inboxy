// ReSharper disable UnusedAutoPropertyAccessor.Local

// ReSharper disable UnusedMember.Local
namespace Inboxy.Ticket.Domain
{
    using System;

    /// <summary>
    /// Represent one comment on ticket 
    /// </summary>
    public class TicketComment
    {
        public TicketComment(string comment, int emailId)
        {
            this.Comment = comment;
            this.EmailId = emailId;
            this.CreatedOn = DateTime.UtcNow;
        }

        public TicketComment(string comment)
        {
            this.Comment = comment;
            this.CreatedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Get the creation date.
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        private TicketComment()
        {
        }

        /// <summary>
        /// Get comment
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// Get the id of email if this comment was created from email
        /// </summary>
        public int? EmailId { get; private set; }

        public int Id { get; private set; }

        public virtual Ticket Ticket { get; private set; }

        /// <summary>
        /// Get ticket Id
        /// </summary>
        public int TicketId { get; private set; }


        /// <summary>
        /// Get the value whether this comment is the initial comment for ticket
        /// </summary>
        public bool IsInitial { get; private set; }
    }
}