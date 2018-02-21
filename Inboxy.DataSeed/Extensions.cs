namespace Inboxy.DataSeed
{
	using System.Threading.Tasks;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;

	public static class Extensions
	{
		public static async Task<LinkedFolder> EnsureInbox(this CoreDbContext context, string email, string newItemsFolder, string processedItemsFolder)
		{
			var inbox = new LinkedFolder(email, email, newItemsFolder, processedItemsFolder);
			context.LinkedFolders.Add(inbox);
			await context.SaveChangesAsync();

			return inbox;
		}

		public static async Task<ApplicationUser> EnsureUser(
			this UserManager<ApplicationUser> userManager,
			string email,
			string password,
			params SystemRole[] roles)
		{
			var user = await userManager.Users.SingleOrDefaultAsync(t => t.Email == email);

			if (user == null)
			{
				await userManager.CreateAsync(new ApplicationUser
				{
					UserName = email,
					Email = email
				}, password);

				user = await userManager.Users.SingleAsync(t => t.Email == email);

				foreach (var role in roles)
				{
					if (role.IsDynamicallyAssigned)
					{
						throw new BusinessException($"Cannot add role ${role.Name}, because it is marked as dynamic.");
					}

					await userManager.AddToRoleAsync(user, role.Name);
				}
			}

			return user;
		}
	}
}