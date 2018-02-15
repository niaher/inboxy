namespace Inboxy.Users.Commands
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users;
	using Inboxy.Users.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "login", Label = "Login", Menu = UserMenus.TopLevel)]
	public class Login : IAsyncForm<Login.Request, ReloadResponse>, ISecureHandler
	{
		private readonly SignInManager<ApplicationUser> signInManager;

		public Login(SignInManager<ApplicationUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public async Task<ReloadResponse> Handle(Request model)
		{
			var user = new EmailAddressAttribute().IsValid(model.Email)
				? await this.signInManager.UserManager.FindByEmailAsync(model.Email)
				: await this.signInManager.UserManager.FindByNameAsync(model.Email);
			
			if (user != null)
			{
				var result = await this.signInManager.PasswordSignInAsync(user, model.Password.Value, model.RememberMe, false);

				if (result.Succeeded)
				{
					this.signInManager.Context.User.AddIdentity(new ClaimsIdentity(new List<Claim>
					{
						new Claim(ClaimTypes.UserData, user.Id.ToString())
					}));

					return new ReloadResponse
					{
						Form = typeof(MyAccount).GetFormId()
					};
				}

				if (result.IsLockedOut)
				{
					throw new BusinessException("This account is locked. Please contact administrator.");
				}
			}

			throw new BusinessException("Invalid login attempt.");
		}

		public UserAction GetPermission()
		{
			return UserActions.Login;
		}

		public class Request : IRequest<ReloadResponse>
		{
			[InputField(Required = true, Label = "Email or username")]
			public string Email { get; set; }

			[InputField(Required = true)]
			public Password Password { get; set; }

			[InputField(Label = "Remember me")]
			public bool RememberMe { get; set; }
		}
	}
}
