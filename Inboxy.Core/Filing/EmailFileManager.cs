namespace Inboxy.Core.Filing
{
	using System;
	using System.Collections.Generic;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.Email;
	using Inboxy.Filing;
	using Inboxy.Infrastructure.User;
	using UiMetadataFramework.Basic.Output;

	[EntityFileManager(EntityType = Key)]
	public class EmailFileManager : IEntityFileManager
	{
		public const string Key = "email";
		private readonly EmailPermissionManager emailPermissionManager;
		private readonly EmailRepository repository;
		private readonly UserContext userContext;

		public EmailFileManager(EmailRepository repository, EmailPermissionManager emailPermissionManager, UserContext userContext)
		{
			this.repository = repository;
			this.emailPermissionManager = emailPermissionManager;
			this.userContext = userContext;
		}

		public bool CanDeleteFiles(object entityId, string metTag)
		{
			return this.CanDo(entityId, EmailAction.Edit);
		}

		public bool CanUploadFiles(object entityId)
		{
			return this.CanDo(entityId, EmailAction.Edit);
		}

		public bool CanViewFiles(object entityId)
		{
			return this.CanDo(entityId, EmailAction.View);
		}

		public IEnumerable<FormLink> GetActions(object entityId, string metaTag = null, bool isMultiple = false)
		{
			yield break;
		}

		public IEnumerable<FormLink> GetFileActions(object entityId, int fileId)
		{
			yield break;
		}

		public static string ContextId(object id) => $"{Key}:{id}";

		private bool CanDo(object entityId, EmailAction action)
		{
			var grantId = Convert.ToInt32(entityId);
			var grant = (ImportedEmail)this.repository.Find(grantId);

			return this.emailPermissionManager.CanDo(
				action,
				this.userContext,
				grant);
		}
	}
}