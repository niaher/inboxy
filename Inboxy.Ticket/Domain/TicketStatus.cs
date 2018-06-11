namespace Inboxy.Ticket.Domain
{
    /// <summary>
    /// Represent ticket status, this is configurable by the admins
    /// </summary>
    public class TicketStatus
    {
        private TicketStatus()
        {
        }

        public TicketStatus(string name)
        {
            this.Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LinkedFolderId { get; set; }

        public virtual LinkedFolder LinkedFolder { get; set; }
    }
}