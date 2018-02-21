namespace Inboxy.Core.Security.LinkedFolder
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;

	public class LinkedFolderAction : EntityAction<LinkedFolder, LinkedFolderRole>
	{
		public static LinkedFolderAction Manage = new LinkedFolderAction(nameof(Manage), LinkedFolderRole.Manager);
		
		private LinkedFolderAction(string name, params LinkedFolderRole[] roles) : base(name, roles)
		{
		}
	}
}