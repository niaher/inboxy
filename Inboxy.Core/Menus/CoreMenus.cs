namespace Inboxy.Core.Menus
{
	using System.Collections.Generic;
	using Inboxy.Infrastructure.Forms.Menu;

	public sealed class CoreMenus : IMenuContainer
	{
		public const string System = "System";
		public const string Emails = "Emails";

		public IList<MenuMetadata> GetMenuMetadata()
		{
			return new List<MenuMetadata>
			{
				new MenuMetadata(Emails, 2),
				new MenuMetadata(System, 20)
			};
		}
	}
}
