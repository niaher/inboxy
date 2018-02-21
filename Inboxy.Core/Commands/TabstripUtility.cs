namespace Inboxy.Core.Commands
{
	using System.Collections.Generic;
	using Inboxy.Core.Commands.Email;
	using Inboxy.Core.Commands.Inbox;
	using Inboxy.Infrastructure;
	using UiMetadataFramework.Basic.Output;

	internal static class TabstripUtility
	{
		public static Tabstrip GetInboxTabs(string currentFormId, int inboxId)
		{
			return new Tabstrip
			{
				CurrentTab = currentFormId,
				Tabs = new List<Tab>
				{
					InboxOverview.Button(inboxId, "Overview").AsTab(),
					Users.Button(inboxId, "Users").AsTab()
				}
			};
		}

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
	}
}