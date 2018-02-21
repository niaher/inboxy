namespace Inboxy.Core.Security.LinkedFolder
{
	using Inboxy.Infrastructure.Security;

	public class LinkedFolderRole : Role
	{
		public static LinkedFolderRole Manager = new LinkedFolderRole(nameof(Manager));

		private LinkedFolderRole(string name) : base(name)
		{
		}
	}
}
