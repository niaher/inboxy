namespace Inboxy.Users.Pickers
{
	using System;
	using System.Linq;
	using CPermissions;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Forms.Typeahead;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Users.Security;

	public class RoleTypeaheadRemoteSource : ITypeaheadRemoteSource<RoleTypeaheadRemoteSource.Request, string>,
		ISecureHandler
	{
		private readonly ActionRegister actionRegister;

		public RoleTypeaheadRemoteSource(ActionRegister actionRegister)
		{
			this.actionRegister = actionRegister;
		}

		public UserAction GetPermission()
		{
			return UserActions.ManageUsers;
		}

		public TypeaheadResponse<string> Handle(Request message)
		{
			var manuallyAssignableRoles = this.actionRegister.GetSystemRoles()
				.Where(t => t.IsDynamicallyAssigned == false);

			if (message.GetByIds)
			{
				return manuallyAssignableRoles
					.Where(t => message.Ids.Items.Any(i => i.Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
					.ToList()
					.AsTypeaheadResponse(t => t.Name, t => t.Name);
			}

			return manuallyAssignableRoles
				.ToList()
				.AsTypeaheadResponse(t => t.Name, t => t.Name);
		}

		public class Request : TypeaheadRequest<string>
		{
		}
	}
}
