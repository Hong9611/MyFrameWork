namespace Project.BehaviourTree
{
    /// <summary>
    /// 모든 구체 노드가 상속하는 베이스 클래스입니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 실제 에이전트 타입(예: EnemyAgent).</typeparam>
    public abstract class BaseNode<TOwner> : INode where TOwner : class
    {
        /// <summary>이 트리를 소유하고 있는 에이전트 객체입니다.</summary>
        protected readonly TOwner m_Owner;

        /// <summary>노드들이 공유하는 블랙보드입니다.</summary>
        protected readonly Blackboard m_Blackboard;

        protected BaseNode(TOwner p_Owner, Blackboard p_Blackboard)
        {
            m_Owner = p_Owner;
            m_Blackboard = p_Blackboard;
        }

        public abstract INode.NodeState Evaluate();

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}