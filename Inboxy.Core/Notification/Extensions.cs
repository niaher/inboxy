namespace Inboxy.Core.Notification
{
	using Nofy.Core;
	using Nofy.Core.Model;
	using UiMetadataFramework.Basic.Output;
	using Inboxy.Infrastructure.Domain;

	public static class Extensions
	{
		public static NotificationAction AsNotificationAction(this FormLink btn, string label)
		{
			return new NotificationAction
			{
				Label = label,
				ActionLink = btn.Form
			};
		}

		public static void PublishForUser<T>(
			this NotificationService ns,
			string description,
			T entity,
			string summary,
			NotificationCategory category,
			params NotificationAction[] actions)
			where T : IDomainEntity
		{
			ns.Publish(new Notification(
				description,
				typeof(T).FullName,
				entity.Key.ToString(),
				NotificationRecipientType.Role.Value,
				"User",
				summary,
				category.Id,
				actions));
		}
	}
}
