namespace Inboxy.Infrastructure.Decorators
{
	using MediatR;

	public interface IRequestHandlerDecorator<in TRequest, out TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		IRequestHandler<TRequest, TResponse> InnerCommand { get; }
	}
}
