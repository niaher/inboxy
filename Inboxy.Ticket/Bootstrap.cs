namespace Inboxy.Ticket
{
    using System.Reflection;
    using Inboxy.Infrastructure;

    public class Bootstrap : IAssemblyBootstrapper
    {
        public int Priority { get; } = 70;

        public void Start(DependencyInjectionContainer dependencyInjectionContainer)
        {
            dependencyInjectionContainer.RegisterUiMetadata(typeof(Bootstrap).GetTypeInfo().Assembly);
        }
    }
}