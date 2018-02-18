namespace Inboxy.Core.Commands.Inbox
{
	using System.Threading.Tasks;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.User;
	using MediatR;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "add-inbox", Label = "Add inbox")]
	public class AddInbox : IAsyncForm<AddInbox.Request, RedirectResponse>
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public AddInbox(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<RedirectResponse> Handle(Request message)
		{
			var inbox = new Inbox(
				message.Email,
				message.Name,
				message.NewItemsFolder,
				message.ProcessedItemsFolder);

			var user = await this.context.RegisteredUsers.FindOrExceptionAsync(this.userContext.User.UserId);

			inbox.AddUser(user);

			this.context.Inboxes.Add(inbox);
			await this.context.SaveChangesAsync();

			return InboxOverview.Button(inbox.Id).AsRedirectResponse();
		}

		public static FormLink Button(string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Create new inbox",
				Form = typeof(AddInbox).GetFormId()
			};
		}

		public class Request : IRequest<RedirectResponse>
		{
			[InputField(OrderIndex = 1, Required = true)]
			public string Email { get; set; }

			[InputField(OrderIndex = 5, Required = true)]
			public string Name { get; set; }

			[InputField(OrderIndex = 15, Required = true)]
			public string NewItemsFolder { get; set; }

			[InputField(OrderIndex = 20, Required = true)]
			public string ProcessedItemsFolder { get; set; }
		}
	}
}