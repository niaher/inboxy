namespace Inboxy.Users.Commands
{
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Outputs;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using Inboxy.Infrastructure.User;

	[MyForm(Id = "change-email", Label = "Change email address")]
	public class ChangeEmail : IMyAsyncForm<ChangeEmail.Request, ChangeEmail.Response>, ISecureHandler
	{
		private readonly UserContext userContext;
		private readonly UserManager<ApplicationUser> userManager;

		public ChangeEmail(UserManager<ApplicationUser> userManager, UserContext userContext)
		{
			this.userManager = userManager;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var user = await this.userManager.FindByNameAsync(this.userContext.User.UserName);

			var passwordIsValid = await this.userManager.CheckPasswordAsync(
				user,
				message.Password.Value);

			if (!passwordIsValid)
			{
				throw new BusinessException("The password you specified is no valid.");
			}

			var result = await this.userManager.SetEmailAsync(
				user,
				message.NewEmail);

			result.EnforceSuccess("Failed to change email address.");

			return new Response
			{
				Result = Alert.Success("Email address was changed successfully.")
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
				Label = "Change email",
				Form = typeof(ChangeEmail).GetFormId()
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			public Alert Result { get; set; }
		}

		public class Request : IRequest<Response>
		{
			[InputField(Required = true, Label = "New email address")]
			public string NewEmail { get; set; }

			[InputField(Required = true, Label = "Your account password")]
			public Password Password { get; set; }
		}
	}
}
