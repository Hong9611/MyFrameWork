using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : class, new()
{
    private readonly Stack<T> m_Pool = new Stack<T>();

    private readonly Func<T> m_CreateFunc;
    private readonly Action<T> m_OnGet;
    private readonly Action<T> m_OnRelease;
    private readonly int m_MaxSize;

    public ObjectPool(Func<T> p_CreateFunc, Action<T> p_OnGet = null, Action<T> p_OnRelease = null, int p_MaxSize = 100)
    {
        m_CreateFunc = p_CreateFunc;
        m_OnGet = p_OnGet;
        m_OnRelease = p_OnRelease;
        m_MaxSize = p_MaxSize;
    }

    public T Get()
    {
        T item = m_Pool.Count > 0 ? m_Pool.Pop() : m_CreateFunc();
        m_OnGet?.Invoke(item);
        return item;
    }

    public void Release(T p_Object)
    {
        m_OnRelease?.Invoke(p_Object);
        if (m_Pool.Count < m_MaxSize)
            m_Pool.Push(p_Object);
    }

    public void Clear()
    {
        m_Pool.Clear();
    }

    public int Count => m_Pool.Count;
}