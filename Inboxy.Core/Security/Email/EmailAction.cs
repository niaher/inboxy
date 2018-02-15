namespace Inboxy.Core.Security.Email
{
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure.Security;

	public class EmailAction : EntityAction<ImportedEmail, EmailRole>
	{
		public static EmailAction Edit = new EmailAction(nameof(Edit), EmailRole.Viewer);
		public static EmailAction View = new EmailAction(nameof(View), EmailRole.Viewer);

		private EmailAction(string name, params EmailRole[] roles) : base(name, roles)
		{
		}
	}
}