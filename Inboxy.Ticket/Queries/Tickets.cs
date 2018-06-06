namespace Inboxy.Ticket.Queries
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.EntityFramework;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Menus;
    using Inboxy.Ticket.Security;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    /// <summary>
    /// This form display all tickets related to specific folder
    /// </summary>
    [MyForm(Id = "tickets", Label = "Tickets",PostOnLoad = true,Menu = TicketMenus.Ticket)]
    public class Tickets : IMyAsyncForm<Tickets.Request, Tickets.Response>,
        ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public Tickets(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var userInboxes = await this.context.InboxUsers.Where(t => t.UserId == this.userContext.User.UserId).Select(t=>t.InboxId).ToListAsync();

            var tickets = this.context.Tickets
                .Include(t => t.RequesterUser)
                .Include(t => t.Status)
                .Where(t => userInboxes.Contains(t.InboxId))
                .OrderByDescending(t=>t.Priority)
                .Paginate(t => new Data(t), message.Paginator);

            return new Response
            {
                Tickets = tickets
            };
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
            [PaginatedData(nameof(Request.Paginator), Label = "")]
            public PaginatedData<Data> Tickets { get; set; }
        }

        public class Data
        {
            public Data(Ticket t)
            {
                this.Requester = t.RequesterUser.Name ?? t.RequesterUser.Email;
                this.Priority = t.Priority.ToString();
                this.Requested = t.CreatedOn;
                this.Status = t.Status?.Name;
                this.Subject = t.Subject;
                this.Type = t.Type.ToString();
                this.Actions = TicketDetails.Button(t);
            }

            [OutputField(OrderIndex = 10)]
            public FormLink Actions { get; set; }

            [OutputField(OrderIndex = 7)]
            public string Priority { get; set; }

            [OutputField(OrderIndex = 4)]
            public DateTime Requested { get; set; }

            [OutputField(OrderIndex = 3)]
            public string Requester { get; set; }

            [OutputField(OrderIndex = 9)]
            public string Status { get; set; }

            [OutputField(OrderIndex = 1)]
            public string Subject { get; set; }

            [OutputField(OrderIndex = 2)]
            public string Type { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public Paginator Paginator { get; set; }
        }

        public UserAction GetPermission()
        {
            return DomainActions.ListTicket;
        }
    }
}