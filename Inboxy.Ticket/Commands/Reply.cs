namespace Inboxy.Ticket.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Security;
    using MediatR;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "ticket-reply", Label = "Reply", SubmitButtonLabel = "Reply")]
    public class Reply : IMyAsyncForm<Reply.Request, Reply.Response>, ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public Reply(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var ticket = await this.context.Tickets.FindOrExceptionAsync(message.TicketId);
            ticket.Reply(message.Reply, this.userContext.User.UserId);

            await this.context.SaveChangesAsync();

            return new Response();
        }

        public static FormLink Button(Ticket ticket)
        {
            return new FormLink()
            {
                Label = "Reply",
                Form = typeof(Reply).GetFormId(),
                InputFieldValues = new Dictionary<string, object>()
                {
                    {nameof(Request.TicketId),ticket.Id }
                }
            };
        }

        public UserAction GetPermission()
        {
            return DomainActions.ReplyToTicket;
        }

        public class Request : IRequest<Response>
        {
            [InputField(Required = true)]
            public string Reply { get; set; }

            [InputField(Required = true, Hidden = true)]
            public int TicketId { get; set; }
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
        }
    }
}