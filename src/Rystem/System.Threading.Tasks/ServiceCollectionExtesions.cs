namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtesions
    {

        public static IServiceCollection AddWaitingCurrentThreadWhenUseNoContext(this IServiceCollection services)
        {
            RystemTaskExtensions.WaitCurrentThread = true;
            return services;
        }
    }
}
