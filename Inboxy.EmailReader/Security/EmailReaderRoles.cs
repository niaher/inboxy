namespace Inboxy.EmailReader.Security
{
	using Inboxy.Infrastructure.Security;

	public class EmailReaderRoles : RoleContainer
	{
		public static readonly SystemRole Anyone = new SystemRole(nameof(Anyone));
	}
}