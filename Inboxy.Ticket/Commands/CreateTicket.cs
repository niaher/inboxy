namespace Inboxy.Ticket.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Menus;
    using Inboxy.Ticket.Pickers;
    using Inboxy.Ticket.Security;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "create-ticket", Label = "Create ticket", SubmitButtonLabel = "Create", Menu = TicketMenus.Ticket)]
    public class CreateTicket : IMyAsyncForm<CreateTicket.Request, CreateTicket.Response>,
        ISecureHandler
    {
        private readonly TicketDbContext dbContext;
        private readonly UserContext userContext;

        public CreateTicket(TicketDbContext dbContext, UserContext userContext)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var requesterUser =
                this.dbContext.RequesterUsers
                    .FirstOrDefault(t => t.Email == message.Email && t.LinkedFolderId == message.LinkedFolderId) ??
                new RequesterUser(message.Name, message.Email, message.LinkedFolderId);

            var inbox = await this.dbContext.Inboxes.Where(t=>t.Id ==  message.InboxId.Value && this.userContext.User.InboxIds.Contains(t.Id)).FirstOrDefaultAsync();
            
            var ticket = new Ticket(message.Subject, this.userContext.User.UserId, requesterUser, message.Details.Value, inbox, message.LinkedFolderId);

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
            [InputField(Label = "How we can help?", Required = true, OrderIndex = 3)]
            public TextareaValue Details { get; set; }

            [InputField(Label = "Email", Required = true, OrderIndex = 1)]
            public string Email { get; set; }

            [TypeaheadInputField(typeof(MyInboxesTypeaheadRemoteSource), Label = "Inbox", OrderIndex = -1)]
            public TypeaheadValue<int> InboxId { get; set; }

            [NotField]
            public int LinkedFolderId { get; set; }

            [InputField(Label = "Name", Required = true, OrderIndex = 0)]
            public string Name { get; set; }

            [InputField(Label = "Subject", Required = true, OrderIndex = 2)]
            public string Subject { get; set; }
        }
    }
}