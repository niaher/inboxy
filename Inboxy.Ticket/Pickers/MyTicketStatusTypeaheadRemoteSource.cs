namespace Inboxy.Ticket.Pickers
{
    using System.Linq;
    using CPermissions;
    using Inboxy.Infrastructure;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Forms.Typeahead;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.DataAccess;
    using Inboxy.Ticket.Security;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UiMetadataFramework.Core.Binding;

    [Form(Id = "my-ticketstatus-sources")]
    public class MyTicketStatusTypeaheadRemoteSource : ITypeaheadRemoteSource<MyTicketStatusTypeaheadRemoteSource.Request, int>, ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public MyTicketStatusTypeaheadRemoteSource(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public UserAction GetPermission()
        {
            return DomainActions.ChangeTicketStatus;
        }

        public TypeaheadResponse<int> Handle(Request message)
        {
            var linkedFolders = this.context.LinkedFolders.Where(t => this.userContext.User.InboxIds.Contains(t.InboxId)).Select(t=>t.Id).ToList();

            return this.context.TicketStatuses
                .Where(t => !t.LinkedFolderId.HasValue || linkedFolders.Contains(t.LinkedFolderId.Value))
                .GetForTypeahead(
                    message,
                    t => new TypeaheadItem<int>(t.Name, t.Id),
                    t => message.Ids.Items.Contains(t.Id),
                    t => t.Name.Contains(message.Query) || t.Name.Contains(message.Query));
        }

        public class Request : TypeaheadRequest<int>
        {
        }
    }
}