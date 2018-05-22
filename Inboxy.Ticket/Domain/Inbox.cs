using System;
namespace Inboxy.Ticket.Domain
{
    using System.Collections.Generic;

    public class Inbox
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public IEnumerable<int>
    }
}
