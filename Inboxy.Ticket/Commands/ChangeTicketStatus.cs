namespace Inboxy.Ticket.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Infrastructure;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Forms.Record;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Domain;
    using Inboxy.Ticket.Pickers;
    using Inboxy.Ticket.Security;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "all-grants", PostOnLoad = true, SubmitButtonLabel = "Change")]
    public class ChangeTicketStatus : IMyAsyncForm<ChangeTicketStatus.Request, ChangeTicketStatus.Response>, ISecureHandler
    {
        private readonly TicketDbContext context;

        public ChangeTicketStatus(TicketDbContext context)
        {
            this.context = context;
        }

        public async Task<Response> Handle(Request message)
        {
            var ticket = await this.context.Tickets.Include(t=>t.Status).SingleOrExceptionAsync(t=>t.Id == message.TicketId);

            if (message.Operation?.Value == RecordRequestOperation.Post)
            {
                if(message.TicketStatus == null)
                    throw new BusinessException("Status can't be null");

                var status = await this.context.TicketStatuses.FindAsync(message.TicketStatus.Value);
                ticket.ChangeStatus(status);
                await this.context.SaveChangesAsync();
            }

            return new Response
            {
                TicketStatus = new TypeaheadValue<int>(ticket.Status.Id),
                Metadata = new MyFormResponseMetadata
                {
                    Title = $"Edit ticket {ticket.Id} status"
                }
            };
        }

        public UserAction GetPermission()
        {
            return DomainActions.ChangeTicketStatus;
        }

        public static FormLink Button(Ticket ticket)
        {
            return new FormLink
            {
                Label = "Change status",
                Form = typeof(ChangeTicketStatus).GetFormId(),
                InputFieldValues = new Dictionary<string, object>()
                {
                    { nameof(Request.TicketId), ticket.Id }
                }
            };
        }

        public class Request : RecordRequest<Response>
        {
            [InputField(Required = true, Hidden = true)]
            public int TicketId { get; set; }

            [TypeaheadInputField(typeof(MyTicketStatusTypeaheadRemoteSource), OrderIndex = 0,Required = true)]
            public TypeaheadValue<int> TicketStatus { get; set; }
        }

        public class Response : RecordResponse
        {
            [NotField]
            public TypeaheadValue<int> TicketStatus { get; set; }
        }
    }
}