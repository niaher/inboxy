namespace Inboxy.Infrastructure.User
{
	using Newtonsoft.Json;

	public class UserContextData
	{
		[JsonConstructor]
		public UserContextData(
			[JsonProperty(nameof(UserName))] string userName,
			[JsonProperty(nameof(UserId))] int userId,
			[JsonProperty(nameof(InboxIds))] int[] inboxIds)
		{
			this.UserName = userName;
			this.UserId = userId;
			this.InboxIds = inboxIds;
		}

		public int[] InboxIds { get; }

		public int UserId { get; }

		public string UserName { get; }
	}
}