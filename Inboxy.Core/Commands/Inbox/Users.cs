namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.Inbox;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "inbox-users", PostOnLoad = true)]
	public class Users : IMyAsyncForm<Users.Request, Users.Response>, IAsyncSecureHandler<LinkedFolder, Users.Request, Users.Response>
	{
		private readonly CoreDbContext context;

		public Users(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return InboxAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var inbox = await this.context.LinkedFolders
				.Include(t => t.Users)
				.ThenInclude(t => t.User)
				.SingleOrExceptionAsync(t => t.Id == message.InboxId);

			return new Response
			{
				Tabs = TabstripUtility.GetInboxTabs(typeof(Users).GetFormId(), inbox.Id),
				Actions = new ActionList(AddUser.Button(inbox.Id, "Add user")),
				Users = inbox.Users.Select(t => new UserRow
				{
					Email = t.User.Name,
					Actions = new ActionList(RemoveUser.Button(t.InboxId, t.UserId, "Remove user").WithAction(FormLinkActions.Run))
				}).ToList(),
				Metadata = new MyFormResponseMetadata
				{
					Title = inbox.Name
				}
			};
		}

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Users",
				Form = typeof(Users).GetFormId(),
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

			[NotField]
			public int ContextId => this.InboxId;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[OutputField(OrderIndex = -5)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = -10)]
			public Tabstrip Tabs { get; set; }

			[OutputField(OrderIndex = 1, Label = "")]
			public IEnumerable<UserRow> Users { get; set; }
		}

		public class UserRow
		{
			[OutputField(OrderIndex = 10, Label = "")]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 1)]
			public string Email { get; set; }
		}
	}
}