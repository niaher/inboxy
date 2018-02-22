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
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "remove-inbox-user", Label = "Remove user")]
	public class RemoveUser : IMyAsyncForm<RemoveUser.Request, RemoveUser.Response>, IAsyncSecureHandler<Inbox, RemoveUser.Request, RemoveUser.Response>
	{
		private readonly CoreDbContext context;

		public RemoveUser(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<Inbox> GetPermission()
		{
			return InboxAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var inbox = await this.context.Inboxes
				.Include(t => t.Users)
				.SingleOrExceptionAsync(t => t.Id == message.InboxId);

			var user = await this.context.RegisteredUsers.SingleOrExceptionAsync(t => t.Id == message.UserId);

			inbox.RemoveUser(user);
			await this.context.SaveChangesAsync();

			return new Response();
		}

		public static FormLink Button(int inboxId, int userId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Remove user",
				Form = typeof(RemoveUser).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), inboxId },
					{ nameof(Request.UserId), userId }
				},
				Action = FormLinkActions.Run
			};
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int InboxId { get; set; }

			[InputField(Hidden = true)]
			public int UserId { get; set; }

			[NotField]
			public int ContextId => this.InboxId;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}