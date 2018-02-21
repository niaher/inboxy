namespace Inboxy.Core.Commands.Inbox
{
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Menus;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using MediatR;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "my-inboxes", PostOnLoad = true, Menu = CoreMenus.Emails, Label = "Inboxes")]
	public class MyInboxes : IMyAsyncForm<MyInboxes.Request, MyInboxes.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public MyInboxes(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var inboxes = await this.context.LinkedFolders
				.Where(t => t.Users.Any(x => x.UserId == this.userContext.User.UserId))
				.PaginateAsync(t => new InboxRow
				{
					Email = InboxOverview.Button(t.Id, t.Name),
					Name = t.Name,
					NewItemsFolder = t.NewItemsFolder,
					ProcessedItemsFolder = t.ProcessedItemsFolder
				}, message.Paginator);

			return new Response
			{
				Inboxes = inboxes,
				Actions = new ActionList(AddInbox.Button("Create new inbox"))
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public class Request : IRequest<Response>
		{
			public Paginator Paginator { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.Paginator), Label = "")]
			public PaginatedData<InboxRow> Inboxes { get; set; }
		}

		public class InboxRow
		{
			[OutputField(OrderIndex = 1)]
			public FormLink Email { get; set; }

			[OutputField(OrderIndex = 2)]
			public string Name { get; set; }

			[OutputField(Label = "New items folder", OrderIndex = 5)]
			public string NewItemsFolder { get; set; }

			[OutputField(Label = "Processed items folder", OrderIndex = 10)]
			public string ProcessedItemsFolder { get; set; }
		}
	}
}