namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.LinkedFolder;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "import-emails", Label = "Import emails from the linked folder", PostOnLoad = true)]
	public class ImportEmails : IMyAsyncForm<ImportEmails.Request, ImportEmails.Response>,
		IAsyncSecureHandler<LinkedFolder, ImportEmails.Request, ImportEmails.Response>
	{
		private readonly CoreDbContext context;

		public ImportEmails(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<LinkedFolder> GetPermission()
		{
			return LinkedFolderAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var folder = await this.context.LinkedFolders
				.Include(t => t.Inbox)
				.SingleOrExceptionAsync(t => t.Id == message.LinkedFolderId);

			var repository = new ExchangeRepository(folder.Inbox.Email);

			await repository.Initialize(folder.NewItemsFolder, folder.ProcessedItemsFolder);

			var result = await repository.Read(new Paginator
			{
				PageSize = int.MaxValue,
				PageIndex = 1
			});

			foreach (var item in result.Results)
			{
				var email = new ImportedEmail(folder, item);

				var alreadyAdded = this.context.ImportedEmails.Any(t =>
					t.LinkedFolderId == folder.Id &&
					t.MessageId == email.MessageId);

				if (!alreadyAdded)
				{
					this.context.ImportedEmails.Add(email);
				}
			}

			await this.context.SaveChangesAsync();

			await repository.MoveItemsToProcessedFolder(result.Results.Select(t => t.Id).ToList());

			return new Response();
		}

		public static FormLink Button(int linkedFolderId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Import emails",
				Form = typeof(ImportEmails).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.LinkedFolderId), linkedFolderId }
				},
				Action = FormLinkActions.Run
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
		}
	}
}