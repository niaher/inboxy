namespace Inboxy.EmailReader.Security
{
	using Inboxy.Infrastructure.Security;

	public class EmailReaderActions : ActionContainer
	{
		public static readonly SystemAction ImportEmails = new SystemAction(nameof(ImportEmails), EmailReaderRoles.Anyone);
	}
}