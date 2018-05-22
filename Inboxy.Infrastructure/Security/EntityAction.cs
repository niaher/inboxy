namespace Inboxy.Infrastructure.Security
{
	using System.Collections.Generic;
	using CPermissions;

	public class EntityAction<TContext, TRole> : UserAction<TContext>
	{
		public EntityAction(string name, params TRole[] allowedRole) : base(name)
		{
			this.Role = allowedRole;
		}

		public IEnumerable<TRole> Role { get; }
	}
}
