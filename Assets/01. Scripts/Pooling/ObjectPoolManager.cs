using System;
using System.Collections.Generic;

public interface IObjectPoolManager
{
    ObjectPool<T> GetPool<T>(
        Func<T> p_CreateFunc,
        Action<T> p_OnGet = null,
        Action<T> p_OnRelease = null,
        int p_MaxSize = 100)
        where T : class, new();

    void ClearPool<T>() where T : class, new();
    void ClearAll();
}

public class ObjectPoolManager : IObjectPoolManager
{
    private readonly Dictionary<Type, object> m_Pools = new Dictionary<Type, object>();

    /// <summary>
    /// 지정된 타입의 풀을 반환합니다. 없으면 새로 생성합니다.
    /// </summary>
    public ObjectPool<T> GetPool<T>(
        Func<T> p_CreateFunc,
        Action<T> p_OnGet = null,
        Action<T> p_OnRelease = null,
        int p_MaxSize = 100)
        where T : class, new()
    {
        Type type = typeof(T);

        if (!m_Pools.TryGetValue(type, out object poolObj))
        {
            var newPool = new ObjectPool<T>(p_CreateFunc, p_OnGet, p_OnRelease, p_MaxSize);
            m_Pools[type] = newPool;
            return newPool;
        }

        return poolObj as ObjectPool<T>;
    }

    /// <summary>
    /// 특정 타입의 풀을 비우고 제거합니다.
    /// </summary>
    public void ClearPool<T>() where T : class, new()
    {
        Type type = typeof(T);
        if (m_Pools.TryGetValue(type, out object poolObj))
        {
            (poolObj as ObjectPool<T>)?.Clear();
            m_Pools.Remove(type);
        }
    }

    /// <summary>
    /// 모든 풀을 초기화하고 제거합니다.
    /// </summary>
    public void ClearAll()
    {
        foreach (var poolObj in m_Pools.Values)
        {
            var clearMethod = poolObj.GetType().GetMethod("Clear");
            clearMethod?.Invoke(poolObj, null);
        }

        m_Pools.Clear();
    }
}