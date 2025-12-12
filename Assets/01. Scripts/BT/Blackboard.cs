using System.Collections.Generic;

namespace Project.BehaviourTree
{
    /// <summary>
    /// BT 노드들 간에 공유되는 Key-Value 저장소입니다.
    /// </summary>
    public class Blackboard
    {
        private readonly Dictionary<string, object> m_Data = new Dictionary<string, object>();

        public void SetValue(string p_Key, object p_Value)
        {
            m_Data[p_Key] = p_Value;
        }

        public T GetValue<T>(string p_Key)
        {
            if (m_Data.TryGetValue(p_Key, out object m_Value))
            {
                if (m_Value is T m_CastValue)
                {
                    return m_CastValue;
                }
            }

            return default;
        }

        public bool TryGetValue<T>(string p_Key, out T p_Value)
        {
            if (m_Data.TryGetValue(p_Key, out object m_Value) && m_Value is T m_CastValue)
            {
                p_Value = m_CastValue;
                return true;
            }

            p_Value = default;
            return false;
        }

        public bool HasValue(string p_Key)
        {
            return m_Data.ContainsKey(p_Key);
        }

        public void Remove(string p_Key)
        {
            if (m_Data.ContainsKey(p_Key))
            {
                m_Data.Remove(p_Key);
            }
        }

        public void Clear()
        {
            m_Data.Clear();
        }
    }
}