namespace Inboxy.Core.Commands.Inbox
{
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using MediatR;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "create-inbox", Label = "Create inbox")]
	public class CreateInbox : IMyAsyncForm<CreateInbox.Request, CreateInbox.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public CreateInbox(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var user = await this.context.RegisteredUsers.FindOrExceptionAsync(this.userContext.User.UserId);
			var inbox = new Inbox(message.Email, message.Name, user);

			this.context.Inboxes.Add(inbox);
			await this.context.SaveChangesAsync();

			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public static FormLink Button(string label)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(CreateInbox).GetFormId()
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 1)]
			public string Email { get; set; }

			[InputField(OrderIndex = 5)]
			public string Name { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}