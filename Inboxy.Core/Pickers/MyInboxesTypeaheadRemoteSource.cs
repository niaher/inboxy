namespace Inboxy.Core.Pickers
{
	using System.Linq;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Typeahead;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Core.Binding;

	[Form(Id = "my-inboxes-sources")]
	public class MyInboxesTypeaheadRemoteSource : ITypeaheadRemoteSource<MyInboxesTypeaheadRemoteSource.Request, int>,
		ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public MyInboxesTypeaheadRemoteSource(UserContext userContext, CoreDbContext context)
		{
			this.userContext = userContext;
			this.context = context;
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public TypeaheadResponse<int> Handle(Request message)
		{
			return this.context.Inboxes
				.Where(t => t.Users.Any(x => x.UserId == this.userContext.User.UserId))
				.GetForTypeahead(
					message,
					t => new TypeaheadItem<int>(t.Name, t.Id),
					t => message.Ids.Items.Contains(t.Id),
					t => t.Name.Contains(message.Query) || t.Email.Contains(message.Query));
		}

		public class Request : TypeaheadRequest<int>
		{
		}
	}
}