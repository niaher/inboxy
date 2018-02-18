namespace Inboxy.Core.Commands.Email
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Menus;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using MediatR;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;

	[MyForm(Id = "my-emails", Label = "Imported emails", PostOnLoad = true, SubmitButtonLabel = "Search", Menu = CoreMenus.Emails)]
	public class MyEmails : IMyAsyncForm<MyEmails.Request, MyEmails.Response>,
		ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public MyEmails(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var emails = await this.context.ImportedEmails
				.Where(t => t.Inbox.Users.Any(x => x.UserId == this.userContext.User.UserId))
				.PaginateAsync(message.EmailPaginator);

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