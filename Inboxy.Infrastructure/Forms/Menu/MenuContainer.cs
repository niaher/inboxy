namespace Inboxy.Infrastructure.Forms.Menu
{
	using System.Collections.Generic;

	public interface IMenuContainer
	{
		IList<MenuMetadata> GetMenuMetadata();
	}
}
