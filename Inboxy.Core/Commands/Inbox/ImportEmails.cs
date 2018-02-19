namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.Inbox;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "import-emails", Label = "Import emails from inbox", PostOnLoad = true)]
	public class ImportEmails : IMyAsyncForm<ImportEmails.Request, ImportEmails.Response>,
		IAsyncSecureHandler<Inbox, ImportEmails.Request, ImportEmails.Response>
	{
		private readonly CoreDbContext context;

		public ImportEmails(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<Inbox> GetPermission()
		{
			return InboxAction.Manage;
		}

		public async Task<Response> Handle(Request message)
		{
			var inbox = await this.context.Inboxes.FindOrExceptionAsync(message.InboxId);

			var repository = new ExchangeRepository(inbox.Email);
			await repository.Initialize(inbox.NewItemsFolder, inbox.ProcessedItemsFolder);

			var result = await repository.Read(new Paginator
			{
				PageSize = int.MaxValue,
				PageIndex = 1
			});

			foreach (var item in result.Results)
			{
				var email = new ImportedEmail(
					inbox,
					item.Id.UniqueId,
					item.From.Address,
					item.Subject,
					item.Body,
					item.DateTimeReceived);

				var alreadyAdded = this.context.ImportedEmails.Any(t => 
					t.InboxId == inbox.Id &&
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

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Import emails",
				Form = typeof(ImportEmails).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), inboxId }
				},
				Action = FormLinkActions.Run
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
		}
	}
}