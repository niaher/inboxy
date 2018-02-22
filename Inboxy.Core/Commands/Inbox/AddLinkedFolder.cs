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
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-linked-folder", Label = "Add linked folder")]
	public class AddLinkedFolder : IMyAsyncForm<AddLinkedFolder.Request, AddLinkedFolder.Response>,
		IAsyncSecureHandler<Inbox, AddLinkedFolder.Request, AddLinkedFolder.Response>
	{
		private readonly CoreDbContext context;

		public AddLinkedFolder(CoreDbContext context)
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

			var folder = inbox.AddFolder(
				message.Name,
				message.NewItemsFolder,
				message.ProcessedItemsFolder);

			this.context.LinkedFolders.Add(folder);
			await this.context.SaveChangesAsync();

			return new Response();
		}

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Link folder",
				Form = typeof(AddLinkedFolder).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), inboxId }
				}
			};
		}

		public class Response : MyFormResponse
		{
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int InboxId { get; set; }

			[InputField(OrderIndex = 5, Required = true, Label = "Name")]
			public string Name { get; set; }

			[InputField(OrderIndex = 15, Required = true, Label = "New items folder")]
			public string NewItemsFolder { get; set; }

			[InputField(OrderIndex = 20, Required = true, Label = "Processed items folder")]
			public string ProcessedItemsFolder { get; set; }

			[NotField]
			public int ContextId => this.InboxId;
		}
	}
}