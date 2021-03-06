﻿namespace Inboxy.Core.Commands.Email
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.Commands.LinkedFolder;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Forms;
	using Inboxy.Core.Menus;
	using Inboxy.Core.Pickers;
	using Inboxy.Core.Security;
    using Inboxy.Help;
    using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

    [Documentation(DocumentationPlacement.Inline,DocumentationSourceType.String,"This is help text for this form","MyEmails.md")]
    [MyForm(Id = "my-emails", Label = "Emails", PostOnLoad = true, SubmitButtonLabel = "Search", Menu = CoreMenus.Emails)]
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
				.Include(t => t.LinkedFolder)
				.Where(t => t.LinkedFolder.Inbox.Users.Any(x => x.UserId == this.userContext.User.UserId))
				.PaginateAsync(message.EmailPaginator);

			return new Response
			{
				Emails = emails.Transform(t => new EmailItem
				{
					Subject = EmailDetails.Button(t.Id, t.Subject),
					LinkedFolder = LinkedFolderOverview.Button(t.LinkedFolderId, t.LinkedFolder.Name),
					From = t.From,
					ReceivedOn = t.ReceivedOn
				})
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTool;
		}

		public static FormLink Button(string label, int? inboxId = null)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(MyEmails).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), new TypeaheadValue<int?>(inboxId) }
				}
			};
		}

		public class Request : IRequest<Response>
		{
			public Paginator EmailPaginator { get; set; }

			[TypeaheadInputField(typeof(MyInboxesTypeaheadRemoteSource), Label = "Inbox", OrderIndex = 1)]
			public TypeaheadValue<int?> InboxId { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[PaginatedData(nameof(Request.EmailPaginator), Label = "")]
			public PaginatedData<EmailItem> Emails { get; set; }
		}

		public class EmailItem
		{
			[OutputField(OrderIndex = 5)]
			public string From { get; set; }

			[OutputField(OrderIndex = 15, Label = "Linked folder")]
			public FormLink LinkedFolder { get; set; }

			[OutputField(OrderIndex = 10, Label = "Received on")]
			[DateTimeFormat(DateTimeFormat.DateAndTime)]
			public DateTime ReceivedOn { get; set; }

			[OutputField(OrderIndex = 1)]
			public FormLink Subject { get; set; }
		}
	}
}