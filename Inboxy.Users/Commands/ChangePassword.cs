namespace Inboxy.Users.Commands
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.ClientFunctions;
	using Inboxy.Infrastructure.Forms.Outputs;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using Inboxy.Infrastructure.User;

	[MyForm(Id = "change-password", Label = "Change account password")]
	public class ChangePassword : IMyAsyncForm<ChangePassword.Request, ChangePassword.Response>, ISecureHandler
	{
		private readonly UserContext userContext;
		private readonly UserManager<ApplicationUser> userManager;

		public ChangePassword(UserManager<ApplicationUser> userManager, UserContext userContext)
		{
			this.userManager = userManager;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var user = await this.userManager.FindByNameAsync(this.userContext.User.UserName);

			var result = await this.userManager.ChangePasswordAsync(
				user,
				message.OldPassword.Value,
				message.NewPassword.Value);

			result.EnforceSuccess("Failed to change password.");

			return new Response
			{
				Result = Alert.Success("Password was changed successfully."),
				Metadata = new MyFormResponseMetadata
				{
					FunctionsToRun = new List<ClientFunctionMetadata>
					{
						new GrowlNotification("Password was changed successfully.", GrowlNotification.Styles.Success).GetClientFunctionMetadata()
					}
				}
			};
		}

		public UserAction GetPermission()
		{
			return UserActions.ManageMyAccount;
		}

		public static FormLink Button()
		{
			return new FormLink
			{
				Label = "Change password",
				Form = typeof(ChangePassword).GetFormId()
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			public Alert Result { get; set; }
		}

		public class Request : IRequest<Response>
		{
			[InputField(Required = true, Label = "Current password")]
			public Password OldPassword { get; set; }

			[InputField(Required = true, Label = "New password")]
			public Password NewPassword { get; set; }
		}
	}
}
