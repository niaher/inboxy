namespace Inboxy.Ticket.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "Create-Ticket-From-Email")]
    public class CreateTicketFromEmail : IMyAsyncForm<CreateTicketFromEmail.Request, CreateTicketFromEmail.Response>
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public CreateTicketFromEmail(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Response> Handle(Request message)
        {
            var email = await this.context.Emails
                .Include(t=>t.LinkedFolder)
                .ThenInclude(t=>t.Inbox)
                .FirstOrDefaultAsync(t=>t.Id == message.EmailId);

            var requesterUser =
                this.context.RequesterUsers
                    .FirstOrDefault(t => t.Email == email.From && t.LinkedFolderId == email.LinkedFolderId) ??
                new RequesterUser(email.From, email.From, email.LinkedFolderId);

            var ticket = new Ticket(email.Subject, this.userContext.User.UserId, requesterUser, email.Body, email.LinkedFolder.Inbox,
                email.LinkedFolderId);

            this.context.Tickets.Add(ticket);
            await this.context.SaveChangesAsync();
            return new Response();
        }

        public static FormLink Button(int emailId)
        {
            return new FormLink
            {
                Label = "Create ticket",
                Form = typeof(CreateTicketFromEmail).GetFormId(),
                InputFieldValues = new Dictionary<string, object>()
                {
                    { nameof(Request.EmailId), emailId }
                },
                Action = FormLinkActions.Run
            };
        }

        public class Request : IRequest<Response>
        {
            [InputField(Required = true, Hidden = true)]
            public int EmailId { get; set; }
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
        }
    }
}