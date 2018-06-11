namespace Inboxy.Ticket.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Security;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;

    [MyForm(Id = "ticket-statuses",Label = "Ticket status",PostOnLoad = true, Menu = Menus.TicketMenus.Ticket)]
    public class TicketStatuses : IMyAsyncForm<TicketStatuses.Request, TicketStatuses.Response>, ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public TicketStatuses(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var linkedFolders = await this.context.LinkedFolders.Where(t => this.userContext.User.InboxIds.Contains(t.InboxId)).Select(t => t.Id).ToListAsync();

            var statuses = this.context.TicketStatuses
                .Include(t => t.LinkedFolder)
                .Where(t =>
                    !t.LinkedFolderId.HasValue ||
                    linkedFolders.Contains(t.LinkedFolderId.Value)
                ).Select(t => new StatusItem(t)).ToList();

            return new Response()
            {
                Statuses = statuses
            };
        }

        public UserAction GetPermission()
        {
            return DomainActions.ManageTicketStatuses;
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
            public IEnumerable<StatusItem> Statuses { get; set; }
        }

        public class StatusItem
        {
            public StatusItem(TicketStatus status)
            {
                this.Status = status.Name;
                this.Inbox = status.LinkedFolder?.Name;
            }

            public ActionList Actions { get; set; }
            public string Inbox { get; set; }

            public string Status { get; set; }
        }

        public class Request : IRequest<Response>
        {
        }
    }
}