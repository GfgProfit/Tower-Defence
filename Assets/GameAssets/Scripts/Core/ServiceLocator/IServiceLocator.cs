public interface IServiceLocator
{
    void Register<TService>(TService service) where TService : class;
    TService Resolve<TService>() where TService : class;
    bool TryResolve<TService>(out TService service) where TService : class;
}