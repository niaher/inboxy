namespace Inboxy.Web
{
	using StructureMap.TypeRules;
	using Inboxy.Infrastructure;

	public class Bootstrap : IAssemblyBootstrapper
	{
		public int Priority { get; } = 0;

		public void Start(DependencyInjectionContainer dependencyInjectionContainer)
		{
			dependencyInjectionContainer.RegisterUiMetadata(this.GetType().GetAssembly());
		}
	}
}
