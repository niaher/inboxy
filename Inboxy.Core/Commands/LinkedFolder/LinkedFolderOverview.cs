namespace Inboxy.Core.Commands.LinkedFolder
{
	using System;
	using System.Threading.Tasks;
	using CPermissions;
	using Inboxy.Core.Domain;
	using Inboxy.Core.Security.LinkedFolder;
	using Inboxy.Infrastructure.Forms;
	using Inboxy.Infrastructure.Security;
	using MediatR;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "linked-folder", PostOnLoad = true)]
	public class LinkedFolderOverview : IMyAsyncForm<LinkedFolderOverview.Request, LinkedFolderOverview.Response>,
		IAsyncSecureHandler<LinkedFolder, LinkedFolderOverview.Request, LinkedFolderOverview.Response>
	{
		public UserAction<LinkedFolder> GetPermission()
		{
			return LinkedFolderAction.Manage;
		}

		public Task<Response> Handle(Request message)
		{
			throw new NotImplementedException();
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int LinkedFolderId { get; set; }

			[NotField]
			public int ContextId => this.LinkedFolderId;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}