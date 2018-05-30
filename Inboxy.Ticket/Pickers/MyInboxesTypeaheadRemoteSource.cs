namespace Inboxy.Ticket.Pickers
{
    using CPermissions;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Forms.Typeahead;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Ticket.DataAccess;
    using System.Linq;
    using Inboxy.Infrastructure;
    using Inboxy.Infrastructure.User;
    using Inboxy.Ticket.Security;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UiMetadataFramework.Core.Binding;

    [Form(Id = "my-ticketinboxes-sources")]
    public class MyInboxesTypeaheadRemoteSource: ITypeaheadRemoteSource<MyInboxesTypeaheadRemoteSource.Request, int>,ISecureHandler
    {
        private readonly TicketDbContext context;
        private readonly UserContext userContext;

        public MyInboxesTypeaheadRemoteSource(TicketDbContext context, UserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public class Request : TypeaheadRequest<int>
        {
        }

        public TypeaheadResponse<int> Handle(Request message)
        {
            return this.context.Inboxes
                .Where(t => t.Users.Any(u=>u.UserId == this.userContext.User.UserId))
                .GetForTypeahead(
                    message,
                    t => new TypeaheadItem<int>(t.Name, t.Id),
                    t => message.Ids.Items.Contains(t.Id),
                    t => t.Name.Contains(message.Query) || t.Email.Contains(message.Query));
        }

        public UserAction GetPermission()
        {
            return DomainActions.GetInboxes;
        }
    }
}
