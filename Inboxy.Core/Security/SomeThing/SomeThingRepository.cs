namespace Inboxy.Core.Security.SomeThing
{
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(SomeThing))]
	public class SomeThingRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public SomeThingRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.SomeThings.SingleOrException(t => t.Id == entityId);
		}
	}
}
