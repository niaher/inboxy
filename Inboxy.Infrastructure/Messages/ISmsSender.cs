namespace Inboxy.Infrastructure.Messages
{
	using System.Threading.Tasks;

	public interface ISmsSender
	{
		Task SendSmsAsync(string number, string message);
	}
}
