namespace Inboxy.Users.Commands
{
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users;
	using Inboxy.Users.Security;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "logout", PostOnLoad = true, Label = "Logout", Menu = UserMenus.Account, MenuOrderIndex = 10)]
	public class Logout : IAsyncForm<Logout.Request, ReloadResponse>, ISecureHandler
	{
		private readonly SignInManager<ApplicationUser> signInManager;

		public Logout(SignInManager<ApplicationUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public async Task<ReloadResponse> Handle(Request message)
		{
			await this.signInManager.SignOutAsync();

			return new ReloadResponse
			{
				Form = typeof(Login).GetFormId()
			};
		}

		public UserAction GetPermission()
		{
			return UserActions.Logout;
		}

		public class Request : IRequest<ReloadResponse>
		{
		}
	}
}
