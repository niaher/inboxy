namespace Inboxy.Users.Commands
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Record;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users.Pickers;
	using Inboxy.Users.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;
	
	[MyForm(Id = "edit-user", PostOnLoad = true, Label = "Edit user", SubmitButtonLabel = "Save changes")]
	public class EditUser : IMyAsyncForm<EditUser.Request, EditUser.Response>, ISecureHandler
	{
		private readonly RoleManager<ApplicationRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;

		public EditUser(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		public static FormLink Button(int userId)
		{
			return new FormLink
			{
				Form = typeof(EditUser).GetFormId(),
				Label = "Edit",
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), userId }
				}
			};
		}

		public async Task<Response> Handle(Request message)
		{
			var user = this.userManager.Users
				.Include(t => t.Roles)
				.ThenInclude(t => t.Role)
				.SingleOrDefault(t => t.Id == message.Id);

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				var existingRoles = user.Roles.Select(t => t.Role.Name).ToList();
				await this.userManager.RemoveFromRolesAsync(user, existingRoles);

				await this.roleManager.EnsureRoles(message.Roles?.Items ?? new string[0]);
				await this.userManager.AddToRolesAsync(user, message.Roles?.Items ?? new string[0]);

				var identityResult = await this.userManager.SetEmailAsync(user, message.Email);
				identityResult.EnforceSuccess("Failed to save changes.");

				identityResult = await this.userManager.SetUserNameAsync(user, message.UserName);
				identityResult.EnforceSuccess("Failed to save changes.");
			}

			return new Response
			{
				Roles = user.Roles.Select(t => t.Role.Name).AsMultiSelect(),
				Email = user.Email,
				Name = user.UserName,
				Metadata = new MyFormResponseMetadata
				{
					Title = $"Edit user {user.UserName}"
				}
			};
		}

		public UserAction GetPermission()
		{
			return UserActions.ManageUsers;
		}

		public class Request : RecordRequest<Response>
		{
			[BindToOutput(nameof(Response.Email))]
			public string Email { get; set; }

			[InputField(Hidden = true, Required = true)]
			public int Id { get; set; }

			[BindToOutput(nameof(Response.Name))]
			public string UserName { get; set; }

			[BindToOutput(nameof(Response.Roles))]
			[TypeaheadInputField(typeof(RoleTypeaheadInlineSource))]
			public MultiSelect<string> Roles { get; set; }
		}

		public class Response : RecordResponse
		{
			[NotField]
			public string Email { get; set; }

			[NotField]
			public string Name { get; set; }

			[NotField]
			public MultiSelect<string> Roles { get; set; }
		}
	}
}
