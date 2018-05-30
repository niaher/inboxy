namespace Inboxy.Ticket.Domain
{
    using System;
    using System.Collections.Generic;

    public class Inbox
    {
        public DateTime CreatedOn { get; private set; }
        public string Email { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        private readonly List<InboxUser> users = new List<InboxUser>();
        public IEnumerable<InboxUser> Users => this.users.AsReadOnly();
    }
}