namespace Inboxy.Core.Commands.LinkedFolder
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.Commands.Email;
	using Inboxy.Core.Commands.Inbox;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.LinkedFolder;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "linked-folder", PostOnLoad = true)]
	public class LinkedFolderOverview : IMyAsyncForm<LinkedFolderOverview.Request, LinkedFolderOverview.Response>,
		IAsyncSecureHandler<LinkedFolder, LinkedFolderOverview.Request, LinkedFolderOverview.Response>
	{
		private readonly CoreDbContext context;

		public LinkedFolderOverview(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return LinkedFolderAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var linkedFolder = await this.context.LinkedFolders
				.Include(t => t.Inbox)
				.SingleOrExceptionAsync(t => t.Id == message.LinkedFolderId);

			var emailCount = await this.context.ImportedEmails.CountAsync(t => t.LinkedFolderId == message.LinkedFolderId);

			return new Response
			{
				Tabs = TabstripUtility.GetLinkedFolderTabs(typeof(LinkedFolderOverview).GetFormId(), linkedFolder.Id),
				Inbox = InboxOverview.Button(linkedFolder.InboxId, linkedFolder.Inbox.Name),
				NewItemsFolder = linkedFolder.NewItemsFolder,
				ProcessedItemsFolder = linkedFolder.ProcessedItemsFolder,
				EmailCount = MyEmails.Button(emailCount.ToString(), linkedFolder.Id),
				Actions = new ActionList(EditLinkedFolder.Button(linkedFolder.Id), ImportEmails.Button(linkedFolder.Id, "Import emails")),
				Metadata = new MyFormResponseMetadata
				{
					Title = linkedFolder.Name
				}
			};
		}

		public static FormLink Button(int linkedFolderId, string label = null)
		{
			return new FormLink
			{
				Form = typeof(LinkedFolderOverview).GetFormId(),
				Label = label ?? linkedFolderId.ToString(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.LinkedFolderId), linkedFolderId }
				}
			};
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int LinkedFolderId { get; set; }

			[NotField]
			public int ContextId => this.LinkedFolderId;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[OutputField(OrderIndex = -5)]
			public ActionList Actions { get; set; }

			[OutputField(Label = "Imported emails", OrderIndex = 20)]
			public FormLink EmailCount { get; set; }

			[OutputField(Label = "Inbox", OrderIndex = 5)]
			public FormLink Inbox { get; set; }

			[OutputField(Label = "New items folder", OrderIndex = 10)]
			public string NewItemsFolder { get; set; }

			[OutputField(Label = "Processed items folder", OrderIndex = 15)]
			public string ProcessedItemsFolder { get; set; }

			[OutputField(OrderIndex = -10)]
			public Tabstrip Tabs { get; set; }
		}
	}
}