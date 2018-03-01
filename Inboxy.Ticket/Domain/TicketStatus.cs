namespace Inboxy.Ticket.Domain
{
    /// <summary>
    /// Represent ticket status, this is configurable by the admins
    /// </summary>
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}