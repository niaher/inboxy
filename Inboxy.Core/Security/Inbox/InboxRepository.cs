namespace Inboxy.Core.Security.Inbox
{
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(Inbox))]
	public class InboxRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public InboxRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.Inboxes.SingleOrException(t => t.Id == entityId);
		}
	}
}