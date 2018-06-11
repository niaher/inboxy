namespace Inboxy.Ticket.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.Commands;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Security;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "ticket-details", PostOnLoad = true, Label = "Ticket details")]
    public class TicketDetails : IMyAsyncForm<TicketDetails.Request, TicketDetails.Response>,
        ISecureHandler
    {
        private readonly TicketDbContext context;

        public TicketDetails(TicketDbContext context)
        {
            this.context = context;
        }

        public async Task<Response> Handle(Request message)
        {
            var ticket = await this.context.Tickets
                .Include(t => t.RequesterUser)
                .Include(t => t.Status)
                .Include(t => t.Comments)
                .SingleOrExceptionAsync(t => t.Id == message.TicketId);

            return new Response
            {
                Requester = ticket.RequesterUser.Name,
                Status = ticket.GetStatus(),
                Subject = ticket.Subject,
                Type = ticket.Type.ToString(),
                Priority = ticket.Priority.ToString(),
                Requested = ticket.CreatedOn,
                Replies = ticket.Replies().Select(t => new TicketReply(t)),
                Details = ticket.Details().Comment,
                Actions = this.getActions(ticket),
                Reply = Reply.InlineForm(ticket)
            };
        }

        public UserAction GetPermission()
        {
            return DomainActions.ViewTicket;
        }

        private ActionList getActions(Ticket ticket)
        {
            return new ActionList(ChangeTicketStatus.Button(ticket));
        }

        public class Request : IRequest<Response>
        {
            [InputField(Required = true, Hidden = true)]
            public int TicketId { get; set; }
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
            [OutputField(OrderIndex = -5)]
            public ActionList Actions { get; set; }

            [OutputField(OrderIndex = 2)]
            public string Details { get; set; }

            [OutputField(OrderIndex = 10)]
            public string Priority { get; set; }

            [OutputField(OrderIndex = 20)]
            public IEnumerable<TicketReply> Replies { get; set; }

            [OutputField(OrderIndex = 30)]
            public InlineForm Reply { get; set; }

            [OutputField(OrderIndex = 3)]
            public DateTime Requested { get; set; }

            [OutputField(OrderIndex = 4)]
            public string Requester { get; set; }

            [OutputField(OrderIndex = 2)]
            public string Status { get; set; }

            [OutputField(OrderIndex = 1)]
            public string Subject { get; set; }

            [OutputField(Hidden = true)]
            public int TicketId { get; set; }

            [OutputField(OrderIndex = 9)]
            public string Type { get; set; }
        }

        public class TicketReply
        {
            public TicketReply(TicketComment comment)
            {
                this.Comment = comment.Comment;
            }

            [OutputField(OrderIndex = -5)]
            public string Comment { get; set; }
        }
    }
}