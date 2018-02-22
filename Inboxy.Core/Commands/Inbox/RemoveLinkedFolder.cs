namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
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

	[MyForm(Id = "remove-linked-folder", Label = "Remove linked folder", PostOnLoad = true)]
	public class RemoveLinkedFolder : IMyAsyncForm<RemoveLinkedFolder.Request, RemoveLinkedFolder.Response>,
		IAsyncSecureHandler<LinkedFolder, RemoveLinkedFolder.Request, RemoveLinkedFolder.Response>
	{
		private readonly CoreDbContext context;

		public RemoveLinkedFolder(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return LinkedFolderAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var folder = await this.context.LinkedFolders.SingleOrDefaultAsync(t => t.Id == message.Id);

			if (folder != null)
			{
				this.context.RemoveRange(folder);
				await this.context.SaveChangesAsync();
			}

			return new Response();
		}

		public static FormLink Button(int linkedFolderId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Remove",
				Form = typeof(RemoveLinkedFolder).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), linkedFolderId }
				},
				Action = FormLinkActions.Run
			};
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int Id { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}