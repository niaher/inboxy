namespace Inboxy.Ticket.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Menus;
    using Inboxy.Ticket.Security;
    using MediatR;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "create-ticket", Label = "Create ticket", SubmitButtonLabel = "Create", Menu = TicketMenus.Ticket)]
    public class CreateTicket : IMyAsyncForm<CreateTicket.Request, CreateTicket.Response>,
        ISecureHandler
    {
        private readonly TicketDbContext dbContext;

        public CreateTicket(TicketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var requesterUser =
                this.dbContext.RequesterUsers.FirstOrDefault(t => t.Email == message.Email && t.LinkedFolderId == message.LinkedFolderId) ??
                new RequesterUser(message.Email, message.LinkedFolderId);

            var ticketComment = new TicketComment(message.Details.Value);

            var ticket = new Ticket(null, requesterUser, ticketComment, message.LinkedFolderId);

            this.dbContext.Tickets.Add(ticket);

            await this.dbContext.SaveChangesAsync();

            return new Response();
        }

        public UserAction GetPermission()
        {
            return DomainActions.CreateTicket;
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
        }

        public class Request : IRequest<Response>
        {
            [InputField(Label = "How we can help?", Required = true, OrderIndex = 2)]
            public TextareaValue Details { get; set; }

            [InputField(Label = "Email", Required = true, OrderIndex = 1)]
            public string Email { get; set; }

            [NotField]
            public int LinkedFolderId { get; set; }
        }
    }
}