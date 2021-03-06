namespace Inboxy.EmailReader.Commands
{
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.EmailReader.DataAccess;
	using Inboxy.EmailReader.Domain;
	using Inboxy.EmailReader.Security;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Core;

	[MyForm(Id = "import-emails", Label = "Import emails from inbox", PostOnLoad = true)]
	public class ImportEmails : IMyAsyncForm<ImportEmails.Request, ImportEmails.Response>,
		ISecureHandler
	{
		private readonly EmailReaderDbContext context;
		private readonly ExchangeRepository repository;

		public ImportEmails(ExchangeRepository repository, EmailReaderDbContext context)
		{
			this.repository = repository;
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			await this.repository.Initialize("inboxy-new", "inboxy-processed");

			var result = await this.repository.Read(new Paginator
			{
				PageSize = int.MaxValue,
				PageIndex = 1
			});

			foreach (var item in result.Results)
			{
				var email = new Email(
					item.Id.UniqueId,
					item.From.Address,
					item.Subject,
					item.Body,
					item.DateTimeReceived);

				var alreadyAdded = this.context.Emails.Any(t => t.MessageId == email.MessageId);

				if (!alreadyAdded)
				{
					this.context.Emails.Add(email);
				}
			}

			await this.context.SaveChangesAsync();

			await this.repository.MoveItemsToProcessedFolder(result.Results.Select(t => t.Id).ToList());

			return new Response();
		}

		public UserAction GetPermission()
		{
			return EmailReaderActions.ImportEmails;
		}

		public class Request : IRequest<Response>
		{
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}