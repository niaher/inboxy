namespace Inboxy.Core.Security.Email
{
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(ImportedEmail))]
	public class EmailRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public EmailRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.ImportedEmails.SingleOrException(t => t.Id == entityId);
		}
	}
}
