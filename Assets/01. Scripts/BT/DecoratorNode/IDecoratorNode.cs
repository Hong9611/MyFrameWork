namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식을 1개만 가지는 Decorator 노드의 베이스 클래스입니다.
    /// </summary>
    public abstract class IDecoratorNode<TOwner> : BaseNode<TOwner> where TOwner : class
    {
        protected INode m_Child;

        protected IDecoratorNode(TOwner p_Owner, Blackboard p_Blackboard, INode p_Child)
            : base(p_Owner, p_Blackboard)
        {
            m_Child = p_Child;
        }

        public void SetChild(INode p_Child)
        {
            m_Child = p_Child;
        }

        public bool HasChild()
        {
            return m_Child != null;
        }
    }
}