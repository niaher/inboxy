namespace Inboxy.Users
{
	using System.Collections.Generic;
	using Inboxy.Infrastructure.Forms.Menu;

	public sealed class UserMenus : IMenuContainer
	{
		public const string Main = "System";
		public const string Account = "Account";
		public const string TopLevel = "";
		public const string Reports = Main + "/Reports";

		public IList<MenuMetadata> GetMenuMetadata()
		{
			return new List<MenuMetadata>
			{
				new MenuMetadata(TopLevel, 100),
				new MenuMetadata(Main, 90),
				new MenuMetadata(Account, 100),
				new MenuMetadata(Reports, 1)
			};
		}
	}
}
