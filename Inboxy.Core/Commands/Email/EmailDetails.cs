namespace Inboxy.Core.Commands.Email
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.Commands.Inbox;
	using Inboxy.Core.Commands.LinkedFolder;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Forms;
	using Inboxy.Core.Forms.Outputs;
	using Inboxy.Core.Security.Email;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "email", PostOnLoad = true)]
	public class EmailDetails : IMyAsyncForm<EmailDetails.Request, EmailDetails.Response>,
		IAsyncSecureHandler<ImportedEmail, EmailDetails.Request, EmailDetails.Response>
	{
		private readonly CoreDbContext context;

		public EmailDetails(CoreDbContext context)
		{
			this.context = context;
		}

		public UserAction<ImportedEmail> GetPermission()
		{
			return EmailAction.View;
		}

		public async Task<Response> Handle(Request message)
		{
			var email = await this.context.ImportedEmails
				.Include(t => t.LinkedFolder)
				.ThenInclude(t => t.Inbox)
				.SingleOrExceptionAsync(t => t.Id == message.Id);

			return new Response
			{
				Body = new EmailBody(email.Body, email.BodyType),
				From = email.From,
				ReceivedOn = email.ReceivedOn,
				ImportedOn = email.ImportedOn,
				Inbox = InboxOverview.Button(email.LinkedFolder.InboxId, email.LinkedFolder.Inbox.Name),
				LinkedFolder = LinkedFolderOverview.Button(email.LinkedFolderId, email.LinkedFolder.Name),
				Metadata = new MyFormResponseMetadata
				{
					Title = email.Subject
				}
			};
		}

		public static FormLink Button(int emailId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? emailId.ToString(),
				Form = typeof(EmailDetails).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), emailId }
				}
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
			[OutputField(OrderIndex = 30, Label = "")]
			public EmailBody Body { get; set; }

			[OutputField(OrderIndex = 1)]
			public string From { get; set; }

			[OutputField(OrderIndex = 10, Label = "Imported on")]
			[DateTimeFormat(DateTimeFormat.DateAndTime)]
			public DateTime ImportedOn { get; set; }

			[OutputField(OrderIndex = 19)]
			public FormLink Inbox { get; set; }

			[OutputField(OrderIndex = 20)]
			public FormLink LinkedFolder { get; set; }

			[OutputField(OrderIndex = 5, Label = "Received on")]
			[DateTimeFormat(DateTimeFormat.DateAndTime)]
			public DateTime ReceivedOn { get; set; }
		}
	}
}