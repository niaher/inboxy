namespace Inboxy.Core.Commands
{
	using System;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Menus;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;

	[MyForm(Id = "emails", Label = "Imported emails", PostOnLoad = true, SubmitButtonLabel = "Search", Menu = CoreMenus.Emails)]
	public class Emails : IMyAsyncForm<Emails.Request, Emails.Response>,
		ISecureHandler
	{
		private readonly CoreDbContext context;

		public Emails(CoreDbContext context)
		{
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			var emails = await this.context.ImportedEmails.PaginateAsync(message.EmailPaginator);

			return new Response
			{
				Emails = emails.Transform(t => new EmailItem
				{
					Subject = t.Subject,
					From = t.From,
					ReceivedOn = t.ReceivedOn
				})
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public class Request : IRequest<Response>
		{
			public Paginator EmailPaginator { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[PaginatedData(nameof(Request.EmailPaginator), Label = "")]
			public PaginatedData<EmailItem> Emails { get; set; }
		}

		public class EmailItem
		{
			public string From { get; set; }
			public DateTime ReceivedOn { get; set; }
			public string Subject { get; set; }
		}
	}
}