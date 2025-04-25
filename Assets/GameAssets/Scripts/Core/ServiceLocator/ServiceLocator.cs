using System;
using System.Collections.Generic;

public class ServiceLocator : IServiceLocator
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register<TService>(TService service) where TService : class
    {
        Type type = typeof(TService);

        if (_services.ContainsKey(type))
        {
            throw new InvalidOperationException($"Service of type {type.Name} is already registered.");
        }

        _services[type] = service ?? throw new ArgumentNullException(nameof(service), $"Service of type {type.Name} cannot be null.");
    }

    public TService Resolve<TService>() where TService : class
    {
        Type type = typeof(TService);

        if (!_services.TryGetValue(type, out object service))
        {
            throw new KeyNotFoundException($"Service of type {type.Name} is not registered.");
        }

        return (TService)service;
    }

    public bool TryResolve<TService>(out TService service) where TService : class
    {
        Type type = typeof(TService);

        if (_services.TryGetValue(type, out object resolved))
        {
            service = (TService)resolved;
            return true;
        }

        service = null;
        return false;
    }
}