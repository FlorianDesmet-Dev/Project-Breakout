using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> ListServices = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        ListServices[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        return (T)ListServices[typeof(T)];
    }
}
