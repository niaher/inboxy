namespace Inboxy.Ticket.Queries
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Security.Inbox;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "tickets", Label = "Tickets",PostOnLoad = true)]
    public class Tickets : IMyAsyncForm<Tickets.Request, Tickets.Response>,
        IAsyncSecureHandler<Inbox, Tickets.Request, Tickets.Response>
    {
        private readonly TicketDbContext context;

        public Tickets(TicketDbContext context)
        {
            this.context = context;
        }

        public UserAction<Inbox> GetPermission()
        {
            return InboxAction.ManageTickets;
        }

        public async Task<Response> Handle(Request message)
        {
            var tickets = await this.context.Tickets
                .Include(t => t.RequesterUser)
                .Include(t => t.Status)
                .Where(t => t.LinkedFolderId == message.InboxId)
                .PaginateAsync(t => new Data(t), message.Paginator);

            return new Response()
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
                this.Priority = t.Priority;
                this.Requested = t.CreatedOn;
                this.Status = t.Status.Name;
                this.Subject = null;
                this.Type = t.Type;
            }

            [OutputField]
            public TicketPriority Priority { get; set; }

            [OutputField]
            public DateTime Requested { get; set; }

            [OutputField]
            public string Requester { get; set; }

            [OutputField]
            public string Status { get; set; }

            [OutputField]
            public FormLink Subject { get; set; }

            [OutputField]
            public TicketType Type { get; set; }
        }

        public class Request : IRequest<Response>, ISecureHandlerRequest
        {
            [InputField(Required = true, Hidden = true)]
            public int InboxId { get; set; }

            public Paginator Paginator { get; set; }

            [NotField]
            public int ContextId => this.InboxId;
        }
    }
}