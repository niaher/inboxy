// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace Inboxy.Ticket.Domain
{
    /// <summary>
    /// Represent the issuer of the ticket
    /// </summary>
    internal class Requester
    {
        public int Id { get; private set; }

        /// <summary>
        /// Get the requester user
        /// </summary>
        public virtual RequesterUser RequesterUser { get; private set; }

        /// <summary>
        /// Get the requester userId
        /// </summary>
        public int RequesterUserId { get; private set; }

        /// <summary>
        /// Get the ticket request details as comment descrip the ticket 
        /// </summary>
        public virtual TicketComment TicketComment { get; private set; }

        /// <summary>
        /// Get ticket comment Id
        /// </summary>
        public int TicketCommentId { get; private set; }


        private Requester()
        {
            
        }
        public Requester(RequesterUser requesterUser, TicketComment ticketComment)
        {
            this.RequesterUser = requesterUser;
            this.TicketComment = ticketComment;
            this.RequesterUserId = requesterUser.Id;
            this.TicketCommentId = ticketComment.Id;
        }
    }
}