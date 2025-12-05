using System;
using System.Collections.Generic;

public class DiContainer
{
    private readonly Dictionary<Type, Func<DiContainer, object>> m_Factories
        = new Dictionary<Type, Func<DiContainer, object>>();

    private readonly Dictionary<Type, object> m_Singletons
        = new Dictionary<Type, object>();

    // 일반적인 Singleton 등록: 구현 타입이 분리된 경우
    public void RegisterSingleton<TService, TImplementation>()
        where TImplementation : TService
    {
        RegisterSingleton<TService>(p_Container =>
        {
            return (TService)Activator.CreateInstance(typeof(TImplementation));
        });
    }

    // 팩토리로 Singleton 등록
    public void RegisterSingleton<TService>(Func<DiContainer, TService> p_Factory)
    {
        Type serviceType = typeof(TService);

        if (m_Factories.ContainsKey(serviceType))
        {
            throw new InvalidOperationException($"서비스 타입 {serviceType.Name} 은(는) 이미 등록되었습니다.");
        }

        // Singleton은 한 번만 생성해서 m_Singletons에 캐시
        m_Factories[serviceType] = p_Container =>
        {
            if (!m_Singletons.TryGetValue(serviceType, out object instance))
            {
                instance = p_Factory(p_Container);
                m_Singletons[serviceType] = instance;
            }

            return instance;
        };
    }

    // Transient 등록: Resolve 할 때마다 새로 생성
    public void RegisterTransient<TService, TImplementation>()
        where TImplementation : TService
    {
        Type serviceType = typeof(TService);

        if (m_Factories.ContainsKey(serviceType))
        {
            throw new InvalidOperationException($"서비스 타입 {serviceType.Name} 은(는) 이미 등록되었습니다.");
        }

        m_Factories[serviceType] = p_Container =>
        {
            return (TService)Activator.CreateInstance(typeof(TImplementation));
        };
    }

    public TService Resolve<TService>()
    {
        Type serviceType = typeof(TService);

        if (!m_Factories.TryGetValue(serviceType, out Func<DiContainer, object> factory))
        {
            throw new InvalidOperationException($"서비스 타입 {serviceType.Name} 이(가) 컨테이너에 등록되어 있지 않습니다.");
        }

        return (TService)factory(this);
    }
}