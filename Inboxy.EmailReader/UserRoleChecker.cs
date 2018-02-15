namespace Inboxy.EmailReader
{
	using System.Collections.Generic;
	using Inboxy.EmailReader.Security;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class UserRoleChecker : IUserRoleChecker
	{
		/// <inheritdoc />
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			yield return EmailReaderRoles.Anyone;
		}
	}
}