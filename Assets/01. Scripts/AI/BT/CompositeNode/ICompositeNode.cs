using System.Collections.Generic;

namespace Project.BehaviourTree
{
    /// <summary>
    /// 여러 자식을 가지는 Composite 노드의 베이스 클래스입니다.
    /// </summary>
    public abstract class ICompositeNode<TOwner> : BaseNode<TOwner> where TOwner : class
    {
        protected readonly List<INode> m_Children;

        protected ICompositeNode(TOwner p_Owner, Blackboard p_Blackboard)
            : base(p_Owner, p_Blackboard)
        {
            m_Children = new List<INode>();
        }

        public void AddChild(INode p_Child)
        {
            if (p_Child == null)
            {
                return;
            }

            m_Children.Add(p_Child);
        }

        public void RemoveChild(INode p_Child)
        {
            if (p_Child == null)
            {
                return;
            }

            m_Children.Remove(p_Child);
        }

        public void ClearChildren()
        {
            m_Children.Clear();
        }

        public int GetChildCount()
        {
            return m_Children.Count;
        }

        public INode GetChild(int p_Index)
        {
            if (p_Index < 0 || p_Index >= m_Children.Count)
            {
                return null;
            }

            return m_Children[p_Index];
        }
    }
}