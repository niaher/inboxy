namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.Commands.Email;
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
			var emailCount = await this.context.ImportedEmails.CountAsync(t => t.InboxId == message.InboxId);

			return new Response
			{
				Id = inbox.Id,
				Email = inbox.Email,
				NewItemsFolder = inbox.NewItemsFolder,
				ProcessedItemsFolder = inbox.ProcessedItemsFolder,
				EmailCount = MyEmails.Button(emailCount.ToString(), inbox.Id),
				Actions = new ActionList(EditInbox.Button(inbox.Id), ImportEmails.Button(inbox.Id, "Import emails")),
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
				Form = typeof(InboxOverview).GetFormId(),
				Label = label ?? inboxId.ToString(),
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
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[OutputField(Label = "Email", OrderIndex = 5)]
			public string Email { get; set; }

			[OutputField(Label = "Imported emails", OrderIndex = 20)]
			public FormLink EmailCount { get; set; }

			[OutputField(Label = "Id", OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(Label = "New items folder", OrderIndex = 10)]
			public string NewItemsFolder { get; set; }

			[OutputField(Label = "Processed items folder", OrderIndex = 15)]
			public string ProcessedItemsFolder { get; set; }
		}
	}
}