namespace Inboxy.Web
{
	using System.Linq;
	using System.Security.Claims;
	using Inboxy.Core.DataAccess;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.User;
	using Inboxy.Users;
	using Microsoft.AspNetCore.Identity;

	public class AppUserContextAccessor : UserContextAccessor
	{
		private readonly CoreDbContext context;
		private readonly CookieManager cookieManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AppUserContextAccessor(
			SignInManager<ApplicationUser> signInManager,
			CookieManager cookieManager,
			UserRoleCheckerRegister register,
			CoreDbContext context) : base(register)
		{
			this.signInManager = signInManager;
			this.cookieManager = cookieManager;
			this.context = context;
		}

		protected override ClaimsPrincipal GetPrincipal()
		{
			return this.signInManager.Context.User;
		}

		protected override UserContextData GetUserContextData()
		{
			var principal = this.GetPrincipal();

			if (!principal.Identity.IsAuthenticated)
			{
				return null;
			}

			var data = this.cookieManager.GetUserContextData();

			if (data == null)
			{
				var userId = principal.GetUserId();

				if (userId == null)
				{
					throw new ApplicationException("Invalid principal. Cannot authenticate.");
				}

				data = this.GetUserContextDataFromDatabase(userId.Value);

				this.cookieManager.SetUserContextData(data);
			}

			return data;
		}

		private UserContextData GetUserContextDataFromDatabase(int userId)
		{
			var user = this.signInManager.UserManager.Users.SingleOrException(t => t.Id == userId);

			var inboxes = this.context.InboxUsers
				.Where(t => t.UserId == userId)
				.Select(t => t.InboxId)
				.ToArray();

			return new UserContextData(user.UserName, userId, inboxes);
		}
	}
}