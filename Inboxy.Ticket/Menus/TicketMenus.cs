namespace Inboxy.Ticket.Menus
{
    using System.Collections.Generic;
    using Inboxy.Infrastructure.Forms.Menu;

    public sealed class TicketMenus : IMenuContainer
    {
        public const string Ticket = "Ticket";

        public IList<MenuMetadata> GetMenuMetadata()
        {
            return new List<MenuMetadata>
            {
                new MenuMetadata(Ticket, 50)
            };
        }
    }
}