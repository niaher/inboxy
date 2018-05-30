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
        public static TicketComment InitialTicketComment(string comment, int? emailId = null)
        {
            return new TicketComment(comment, emailId)
            {
                IsInitial = true
            };
        }

        public TicketComment(string comment, int? emailId=null,int? userId=null)
        {
            this.EmailId = emailId;
            this.Comment = comment;
            this.CreatedByUserId = userId;
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
        /// Get the user who created this comment, will be null if comment was created by the system
        /// </summary>
        public int? CreatedByUserId { get; set; }


        /// <summary>
        /// Get the value whether this comment is the initial comment for ticket (initializer for the ticket)
        /// </summary>
        public bool IsInitial { get; private set; }
    }
}