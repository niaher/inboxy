namespace Inboxy.DataSeed
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users;
	using Inboxy.Users.Security;
	using Microsoft.AspNetCore.Identity;

	public class DataSeed
	{
		private readonly ActionRegister actionRegister;
		private readonly CoreDbContext context;
		private readonly RoleManager<ApplicationRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;

		public DataSeed(
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			ActionRegister actionRegister,
			CoreDbContext context)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.actionRegister = actionRegister;
			this.context = context;
		}

		public async Task Seed(bool productionEnvironment = false)
		{
			await this.EnsureRoles();

			if (!productionEnvironment)
			{
				await this.SeedUsers();
				await this.SeedInboxes();
			}
		}

		private async Task EnsureInbox(string inboxEmail, params string[] adminEmails)
		{
			var users = new List<RegisteredUser>();
			foreach (var adminEmail in adminEmails)
			{
				var user = await this.userManager.EnsureUser(adminEmail, "Password1", CoreRoles.ToolUser);
				var registeredUser = await this.context.RegisteredUsers.FindAsync(user.Id);
				users.Add(registeredUser);
			}

			await this.context.EnsureInbox(inboxEmail, users.ToArray());

			await this.context.SaveChangesAsync();
		}

		private async Task EnsureRoles()
		{
			var manuallyAssignableSystemRoles = this.actionRegister.GetSystemRoles()
				.Where(t => !t.IsDynamicallyAssigned)
				.ToArray();

			await this.roleManager.EnsureRoles(manuallyAssignableSystemRoles);
		}

		private async Task SeedInboxes()
		{
			await this.EnsureInbox("ict.infrastructure@example.com", "andreh@example.com", "karsten@example.com");
			await this.EnsureInbox("helpdesk@example.com", "nikita@example.com", "omar@example.com");
			await this.EnsureInbox("ictdev@example.com", "nikita@example.com", "mohammed@example.com", "nehad@example.com");
		}

		private async Task SeedUsers()
		{
			await this.userManager.EnsureUser("admin@example.com", "Password1", UserManagementRoles.UserAdmin);
		}
	}
}