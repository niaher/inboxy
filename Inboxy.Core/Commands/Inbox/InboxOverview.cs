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

	[MyForm(Id = "inbox", PostOnLoad = true)]
	public class InboxOverview : IMyAsyncForm<InboxOverview.Request, InboxOverview.Response>,
		IAsyncSecureHandler<Inbox, InboxOverview.Request, InboxOverview.Response>
	{
		private readonly CoreDbContext context;

		public InboxOverview(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<Inbox> GetPermission()
		{
			return InboxAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var inbox = await this.context.Inboxes.SingleOrExceptionAsync(t => t.Id == message.InboxId);

			return new Response
			{
				Name = inbox.Name,
				Actions = new ActionList(EditInbox.Button(inbox.Id, "Edit")),
				Tabs = TabstripUtility.GetInboxTabs(typeof(InboxOverview).GetFormId(), inbox.Id),
				Metadata = new MyFormResponseMetadata
				{
					Title = inbox.Email
				}
			};
		}

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Overview",
				Form = typeof(InboxOverview).GetFormId(),
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

			[OutputField(OrderIndex = 1)]
			public string Name { get; set; }

			[OutputField(OrderIndex = -10)]
			public Tabstrip Tabs { get; set; }
		}
	}
}