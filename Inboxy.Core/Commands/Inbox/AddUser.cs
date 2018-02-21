namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.Inbox;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-inbox-user", Label = "Add user")]
	public class AddUser : IMyAsyncForm<AddUser.Request, AddUser.Response>, IAsyncSecureHandler<LinkedFolder, AddUser.Request, AddUser.Response>
	{
		private readonly CoreDbContext context;

		public AddUser(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return InboxAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var inbox = await this.context.LinkedFolders.SingleOrExceptionAsync(t => t.Id == message.InboxId);

			var user = await this.context.RegisteredUsers.SingleOrExceptionAsync(t => t.Name == message.UserEmail);
			inbox.AddUser(user);

			await this.context.SaveChangesAsync();

			return new Response();
		}

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Add user",
				Form = typeof(AddUser).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), inboxId }
				}
			};
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int InboxId { get; set; }

			[InputField(Label = "User's email address", Required = true)]
			public string UserEmail { get; set; }

			[NotField]
			public int ContextId => this.InboxId;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}