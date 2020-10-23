namespace Novibet.Service.IpGeolocation.Common.Interfaces
{
    /// <summary>
    /// Abstract service proxy factory
    /// </summary>
    /// <typeparam name="TSettings">The <see cref="IServiceProxySettings"/> to build the service proxy with</typeparam>
    public interface IServiceProxyFactory<TSettings>
        where TSettings : IServiceProxySettings
    {
        IServiceProxy Create();
    }
}
