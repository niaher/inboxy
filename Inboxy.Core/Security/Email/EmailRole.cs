namespace Inboxy.Core.Security.Email
{
	using Inboxy.Infrastructure.Security;

	public class EmailRole : Role
	{
		public static EmailRole Viewer = new EmailRole(nameof(Viewer));

		private EmailRole(string name) : base(name)
		{
		}
	}
}
