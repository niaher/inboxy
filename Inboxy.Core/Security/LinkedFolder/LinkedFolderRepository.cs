namespace Inboxy.Core.Security.LinkedFolder
{
	using Inboxy.Core.DataAccess;
	using Inboxy.Core.Domain;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(LinkedFolder))]
	public class LinkedFolderRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public LinkedFolderRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.LinkedFolders.SingleOrException(t => t.Id == entityId);
		}
	}
}