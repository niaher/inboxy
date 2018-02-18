// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Inboxy.Core.Domain
{
	using Inboxy.Infrastructure.Domain;

	/// <summary>
	/// Represents user registered in the system.
	/// </summary>
	public class RegisteredUser : DomainEntityWithKeyInt32
	{
		private RegisteredUser()
		{
			// This constructor is private, because we are not supposed to create new users
			// from this library. This assembly can only read existing data.
		}

		public string Name { get; private set; }
	}
}