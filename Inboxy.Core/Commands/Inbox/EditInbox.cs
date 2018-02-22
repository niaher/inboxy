namespace Inboxy.Core.Commands.Inbox
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.Inbox;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Record;
	using Inboxy.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-inbox", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit inbox", SubmitButtonLabel = "Save changes")]
	public class EditInbox : IMyAsyncForm<EditInbox.Request, EditInbox.Response>, IAsyncSecureHandler<Inbox, EditInbox.Request, EditInbox.Response>
	{
		private readonly CoreDbContext context;

		public EditInbox(CoreDbContext context)
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

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				inbox.ChangeName(message.Name);
				inbox.ChangeEmail(message.Email);

				await this.context.SaveChangesAsync();
			}

			return new Response
			{
				Email = inbox.Email,
				Name = inbox.Name
			};
		}

		public static FormLink Button(int inboxId, string label = null)
		{
			return new FormLink
			{
				Label = label ?? "Edit",
				Form = typeof(EditInbox).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.InboxId), inboxId }
				}
			};
		}

		public class Request : RecordRequest<Response>, ISecureHandlerRequest
		{
			[InputField(OrderIndex = 1, Required = true)]
			[BindToOutput(nameof(Response.Email))]
			public string Email { get; set; }

			[InputField(Hidden = true)]
			public int InboxId { get; set; }

			[InputField(OrderIndex = 5, Required = true)]
			[BindToOutput(nameof(Response.Name))]
			public string Name { get; set; }

			[NotField]
			public int ContextId => this.InboxId;
		}

		public class Response : RecordResponse
		{
			[NotField]
			public string Email { get; set; }

			[NotField]
			public string Name { get; set; }
		}
	}
}