namespace Inboxy.EmailReader.Commands
{
	using System;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.EmailReader;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using Microsoft.Extensions.Options;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Menu = CoreMenus.Main, Id = "emails", Label = "Emails", PostOnLoad = true)]
	public class Emails : IMyAsyncForm<Emails.Request, Emails.Response>,
		ISecureHandler
	{
		private readonly InboxConfig inboxConfig;

		public Emails(IOptions<InboxConfig> inboxConfig)
		{
			this.inboxConfig = inboxConfig.Value;
		}

		public async Task<Response> Handle(Request message)
		{
			var repository = new ExchangeRepository(this.inboxConfig);

			var data = await repository.Read(message.EmailPaginator);
			var emails = data.Transform(t => new EmailItem
			{
				From = t.From.Address,
				ReceivedOn = t.DateTimeReceived,
				Subject = t.Subject
			});

			return new Response
			{
				Emails = emails
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public class Response : MyFormResponse
		{
			[PaginatedData(nameof(Request.EmailPaginator), Label = "")]
			public PaginatedData<EmailItem> Emails { get; set; }
		}

		public class Request : IRequest<Response>
		{
			public Paginator EmailPaginator { get; set; }
		}

		public class EmailItem
		{
			[OutputField(Label = "From", OrderIndex = 5)]
			public string From { get; set; }

			[OutputField(Label = "Received on", OrderIndex = 1)]
			public DateTime ReceivedOn { get; set; }

			[OutputField(Label = "Subject", OrderIndex = 10)]
			public string Subject { get; set; }
		}
	}
}