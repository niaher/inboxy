namespace Inboxy.Core.Commands.LinkedFolder
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.LinkedFolder;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Record;
	using Inboxy.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-linked-folder", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit linked folder",
		SubmitButtonLabel = "Save changes")]
	public class EditLinkedFolder : IMyAsyncForm<EditLinkedFolder.Request, EditLinkedFolder.Response>,
		IAsyncSecureHandler<LinkedFolder, EditLinkedFolder.Request, EditLinkedFolder.Response>
	{
		private readonly CoreDbContext context;

		public EditLinkedFolder(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return LinkedFolderAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var folder = await this.context.LinkedFolders.SingleOrExceptionAsync(t => t.Id == message.LinkedFolderId);

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				folder.ChangeName(message.Name);
				folder.ChangeNewItemsFolder(message.NewItemsFolder);
				folder.ChangeProcessedItemsFolder(message.ProcessedItemsFolder);

				await this.context.SaveChangesAsync();
			}

			return new Response
			{
				NewItemsFolder = folder.NewItemsFolder,
				ProcessedItemsFolder = folder.ProcessedItemsFolder,
				Name = folder.Name
			};
		}

		public static FormLink Button(int linkedFolderId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Edit",
				Form = typeof(EditLinkedFolder).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.LinkedFolderId), linkedFolderId }
				}
			};
		}

		public class Request : RecordRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int LinkedFolderId { get; set; }

			[InputField(OrderIndex = 5, Required = true)]
			[BindToOutput(nameof(Response.Name))]
			public string Name { get; set; }

			[InputField(OrderIndex = 15, Required = true, Label = "New items folder")]
			[BindToOutput(nameof(Response.NewItemsFolder))]
			public string NewItemsFolder { get; set; }

			[InputField(OrderIndex = 20, Required = true, Label = "Processed items folder")]
			[BindToOutput(nameof(Response.ProcessedItemsFolder))]
			public string ProcessedItemsFolder { get; set; }

			[NotField]
			public int ContextId => this.LinkedFolderId;
		}

		public class Response : RecordResponse
		{
			[NotField]
			public string Name { get; set; }

			[NotField]
			public string NewItemsFolder { get; set; }

			[NotField]
			public string ProcessedItemsFolder { get; set; }
		}
	}
}