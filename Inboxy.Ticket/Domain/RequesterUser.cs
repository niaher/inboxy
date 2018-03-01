// ReSharper disable UnusedAutoPropertyAccessor.Local

// ReSharper disable UnusedMember.Local

namespace Inboxy.Ticket.Domain
{
    /// <summary>
    /// Represent user details for ticket requester
    /// </summary>
    internal class RequesterUser
    {
        private RequesterUser()
        {
        }

        public RequesterUser(string email, int linkedFolderId)
        {
            this.Email = email;
            this.LinkedFolderId = linkedFolderId;
        }

        public string Email { get; private set; }
        public int Id { get; private set; }
        public int LinkedFolderId { get; set; }
    }
}