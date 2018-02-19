namespace Inboxy.Core.Test
{
	using Inboxy.Core.DataAccess;
	using Inboxy.Infrastructure;
	using StructureMap;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;
	using Xunit;

	public class MetadataTest
	{
		[Fact]
		public void MetadataIsConfiguredCorrectly()
		{
			var coreAssembly = typeof(CoreDbContext).Assembly;

			var container = new Container();

			container.Configure(config =>
			{
				config.Scan(_ =>
				{
					_.AssembliesFromApplicationBaseDirectory();
					_.AddAllTypesOf<IAssemblyBootstrapper>();
					_.WithDefaultConventions();
				});
			});

			var binder = new MetadataBinder(new UiMetadataFramework.Core.Binding.DependencyInjectionContainer(t => container.GetInstance(t)));
			binder.RegisterAssembly(typeof(StringInputFieldBinding).Assembly);
			binder.RegisterAssembly(typeof(BusinessException).Assembly);
			binder.RegisterAssembly(coreAssembly);

			var formRegistry = new FormRegister(binder);
			formRegistry.RegisterAssembly(coreAssembly);
		}
	}
}