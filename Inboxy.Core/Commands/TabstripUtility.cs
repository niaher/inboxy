namespace Inboxy.Core.Commands
{
	using System.Collections.Generic;
	using Inboxy.Core.Commands.Email;
	using Inboxy.Core.Commands.Inbox;
	using Inboxy.Core.Commands.LinkedFolder;
	using Inboxy.Infrastructure;
	using UiMetadataFramework.Basic.Output;

	internal static class TabstripUtility
	{
		public static Tabstrip GetEmailTabs(string currentFormId, int emailId)
		{
			return new Tabstrip
			{
				CurrentTab = currentFormId,
				Tabs = new List<Tab>
				{
					EmailDetails.Button(emailId, "Email").AsTab()
				}
			};
		}

		public static Tabstrip GetInboxTabs(string currentFormId, int inboxId)
		{
			return new Tabstrip
			{
				CurrentTab = currentFormId,
				Tabs = new List<Tab>
				{
					InboxOverview.Button(inboxId, "Overview").AsTab(),
					LinkedFolders.Button(inboxId, "Linked folders").AsTab(),
					Users.Button(inboxId, "Users").AsTab()
				}
			};
		}

		public static Tabstrip GetLinkedFolderTabs(string currentFormId, int linkedFolderId)
		{
			return new Tabstrip
			{
				CurrentTab = currentFormId,
				Tabs = new List<Tab>
				{
					LinkedFolderOverview.Button(linkedFolderId, "Overview").AsTab(),
				}
			};
		}
	}
}