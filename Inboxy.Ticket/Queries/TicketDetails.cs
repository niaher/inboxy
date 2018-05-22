namespace Inboxy.Ticket.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Forms.Record;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Security.Ticket;
    using UiMetadataFramework.Basic.EventHandlers;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "Ticket", PostOnLoad = true, PostOnLoadValidation = false, SubmitButtonLabel = "Reply")]
    public class TicketDetails : IMyAsyncForm<TicketDetails.Request, TicketDetails.Response>,
        IAsyncSecureHandler<Ticket, TicketDetails.Request, TicketDetails.Response>
    {
        private readonly TicketDbContext context;

        public TicketDetails(TicketDbContext context)
        {
            this.context = context;
        }

        public UserAction<Ticket> GetPermission()
        {
            return TicketAction.ViewTicket;
        }

        public async Task<Response> Handle(Request message)
        {
            var ticket = this.context.Tickets.FindOrExceptionAsync(message.TicketId);
            return new Response();
        }

        public class Request : RecordRequest<Response>, ISecureHandlerRequest
        {
            [InputField]
            [BindToOutput(nameof(Response.Priority))]
            public TicketPriority Priority { get; set; }

            [InputField(Required = true)]
            public TextareaValue Reply { get; set; }

            [InputField(Required = true, Hidden = true)]
            public int TicketId { get; set; }

            [InputField]
            [BindToOutput(nameof(Response.Type))]
            public TicketType Type { get; set; }

            [NotField]
            public int ContextId => this.TicketId;
        }

        public class Response : RecordResponse
        {
            [OutputField(OrderIndex = -5)]
            public ActionList Actions { get; set; }

            [OutputField]
            public TicketPriority Priority { get; set; }

            [OutputField]
            public IEnumerable<string> Replies { get; set; }

            [OutputField]
            public DateTime Requested { get; set; }

            [OutputField]
            public string Requester { get; set; }

            [OutputField]
            public string Status { get; set; }

            [OutputField]
            public string Subject { get; set; }

            [OutputField]
            public TicketType Type { get; set; }
        }
    }
}