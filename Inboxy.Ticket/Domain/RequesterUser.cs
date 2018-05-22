// ReSharper disable UnusedAutoPropertyAccessor.Local

// ReSharper disable UnusedMember.Local

namespace Inboxy.Ticket.Domain
{
    /// <summary>
    /// Represent user details for ticket requester
    /// </summary>
    public class RequesterUser
    {
        private RequesterUser()
        {
        }

        public RequesterUser(string name, string email, int linkedFolderId)
        {
            this.Name = name;
            this.Email = email;
            this.LinkedFolderId = linkedFolderId;
        }

        public string Email { get; private set; }
        public int Id { get; private set; }
        public int LinkedFolderId { get; set; }

        public string Name { get; private set; }
    }
}