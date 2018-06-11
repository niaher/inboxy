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
    using Inboxy.Ticket.Queries;
    using Inboxy.Ticket.Security;
    using MediatR;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Basic.Response;
    using UiMetadataFramework.Core.Binding;
    using UiMetadataFramework.MediatR;

    [MyForm(Id = "ticket-reply", Label = "Reply", SubmitButtonLabel = "Reply", PostOnLoad = false)]
    public class Reply : IAsyncForm<Reply.Request, ReloadResponse>, ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public Reply(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<ReloadResponse> Handle(Request message)
        {
            var ticket = await this.context.Tickets.FindOrExceptionAsync(message.TicketId);
            ticket.Reply(message.Reply.Value, this.userContext.User.UserId);

            await this.context.SaveChangesAsync();

            return new ReloadResponse()
            {
                Form = typeof(TicketDetails).GetFormId(),
                InputFieldValues = new Dictionary<string, object>()
                {
                    { nameof(TicketDetails.Request.TicketId), message.TicketId }
                },
            };
        }

        public UserAction GetPermission()
        {
            return DomainActions.ReplyToTicket;
        }

        public static FormLink Button(Ticket ticket)
        {
            return new FormLink
            {
                Label = "Reply",
                Form = typeof(Reply).GetFormId(),
                InputFieldValues = new Dictionary<string, object>()
                {
                    { nameof(Request.TicketId), ticket.Id }
                }
            };
        }

        public static InlineForm InlineForm(Ticket ticket)
        {
            return new InlineForm()
            {
                Form = typeof(Reply).GetFormId(),
                InputFieldValues = new Dictionary<string, object>
                {
                    { nameof(Request.TicketId), ticket.Id }
                }
            };
        }

        public class Request : IRequest<ReloadResponse>
        {
            [InputField(Required = true)]
            public TextareaValue Reply { get; set; }

            [InputField(Required = true, Hidden = true)]
            public int TicketId { get; set; }
        }
    }
}