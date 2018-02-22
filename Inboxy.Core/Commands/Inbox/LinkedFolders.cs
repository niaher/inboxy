namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Linq;
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

	[MyForm(Id = "linked-folders", PostOnLoad = true)]
	public class LinkedFolders : IMyAsyncForm<LinkedFolders.Request, LinkedFolders.Response>,
		IAsyncSecureHandler<Inbox, LinkedFolders.Request, LinkedFolders.Response>
	{
		private readonly CoreDbContext context;

		public LinkedFolders(CoreDbContext context)
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
				.Include(t => t.LinkedFolders)
				.SingleOrExceptionAsync(t => t.Id == message.InboxId);

			var folderIds = inbox.LinkedFolders.Select(t => t.Id).ToList();
			var emailCounts = await this.context.ImportedEmails
				.Where(t => folderIds.Contains(t.LinkedFolderId))
				.GroupBy(t => t.LinkedFolderId)
				.Select(t => new
				{
					LinkedFolderId = t.Key,
					EmailCount = t.Count()
				})
				.ToDictionaryAsync(t => t.LinkedFolderId);

			return new Response
			{
				Folders = inbox.LinkedFolders.Select(t => new LinkedFolderRow
				{
					ProcessedItemsFolder = t.ProcessedItemsFolder,
					NewItemsFolder = t.NewItemsFolder,
					EmailCount = MyEmails.Button((emailCounts.GetValueOrDefault(t.Id)?.EmailCount ?? 0).ToString(), t.InboxId),
					Actions = new ActionList(
						RemoveLinkedFolder.Button(t.Id),
						ImportEmails.Button(t.Id, "Import emails"))
				}).ToList(),
				Actions = new ActionList(AddLinkedFolder.Button(inbox.Id, "Add new linked folder")),
				Tabs = TabstripUtility.GetInboxTabs(typeof(LinkedFolders).GetFormId(), inbox.Id),
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
				Label = label ?? "Linked folders",
				Form = typeof(LinkedFolders).GetFormId(),
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

			[OutputField(Label = "")]
			public List<LinkedFolderRow> Folders { get; set; }

			[OutputField(OrderIndex = -10)]
			public Tabstrip Tabs { get; set; }
		}

		public class LinkedFolderRow
		{
			[OutputField(OrderIndex = 30, Label = "")]
			public ActionList Actions { get; set; }

			[OutputField(Label = "Imported emails", OrderIndex = 20)]
			public FormLink EmailCount { get; set; }

			[OutputField(Label = "New items folder", OrderIndex = 10)]
			public string NewItemsFolder { get; set; }

			[OutputField(Label = "Processed items folder", OrderIndex = 15)]
			public string ProcessedItemsFolder { get; set; }
		}
	}
}