namespace Inboxy.Ticket.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Inboxy.Infrastructure;
    using Inboxy.Infrastructure.EntityFramework;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Menus;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "tickets-emails", Label = "Emails", Menu = TicketMenus.Ticket, PostOnLoad = true)]
    public class MyTicketEmails : IMyAsyncForm<MyTicketEmails.Request, MyTicketEmails.Response>
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public MyTicketEmails(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var inboxes = this.userContext.User.InboxIds;
            var linkedFolders =  this.context.LinkedFolders.Where(t => inboxes.Contains(t.InboxId)).Select(t => t.Id).ToList();
            var emails =   this.context.Emails
                .Include(t => t.LinkedFolder)
                .Where(t => linkedFolders.Contains(t.LinkedFolderId))
                .Select(t=>new EmailItem()
                {
                    LinkedFolder = t.LinkedFolder.Name,
                    Subject = t.Subject,
                    From = t.From,
                    ReceivedOn = t.ReceivedOn,
                    Actions = new ActionList(CreateTicketFromEmail.Button(t.Id))
                })
                .Paginate(message.EmailPaginator);

            return new Response
            {
                Emails = emails
            };
        }

        public class EmailItem
        {

            [OutputField(OrderIndex = 20)]
            public ActionList Actions { get; set; }

            [OutputField(OrderIndex = 5)]
            public string From { get; set; }

            [OutputField(OrderIndex = 15, Label = "Linked folder")]
            public string LinkedFolder { get; set; }

            [OutputField(OrderIndex = 10, Label = "Received on")]
            public DateTime ReceivedOn { get; set; }

            [OutputField(OrderIndex = 1)]
            public string Subject { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public Paginator EmailPaginator { get; set; }
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
            [PaginatedData(nameof(Request.EmailPaginator), Label = "")]
            public PaginatedData<EmailItem> Emails { get; set; }
        }
    }
}