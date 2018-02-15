namespace Inboxy.Infrastructure.Forms
{
	using MediatR;
	using Inboxy.Infrastructure.Forms.Typeahead;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.MediatR;

	public interface ITypeaheadRemoteSource<in TRequest, TKey> :
		IForm<TRequest, TypeaheadResponse<TKey>>,
		ITypeaheadRemoteSource
		where TRequest : IRequest<TypeaheadResponse<TKey>>
	{
	}
}
