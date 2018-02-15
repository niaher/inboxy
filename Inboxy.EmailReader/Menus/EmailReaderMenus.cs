namespace Inboxy.EmailReader.Menus
{
	using System.Collections.Generic;
	using Inboxy.Infrastructure.Forms.Menu;

	public sealed class EmailReaderMenus : IMenuContainer
	{
		public const string Emails = "Emails";

		public IList<MenuMetadata> GetMenuMetadata()
		{
			return new List<MenuMetadata>
			{
				new MenuMetadata(Emails, 2)
			};
		}
	}
}
