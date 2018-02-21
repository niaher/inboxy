namespace Inboxy.Core.Security.LinkedFolder
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;
	using Inboxy.Infrastructure.User;

	public class LinkedFolderPermissionManager : EntityPermissionManager<UserContext, LinkedFolderAction, LinkedFolderRole, LinkedFolder>
	{
		public LinkedFolderPermissionManager() : base(new LinkedFolderRoleChecker())
		{
		}
	}
}
