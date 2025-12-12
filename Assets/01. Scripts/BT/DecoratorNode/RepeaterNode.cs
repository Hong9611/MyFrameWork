namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식 노드를 N번 또는 무한 반복시키는 Repeater 데코레이터입니다.
    /// </summary>
    public class RepeaterNode<TOwner> : IDecoratorNode<TOwner> where TOwner : class
    {
        private readonly int m_MaxCount;
        private int m_CurrentCount;

        /// <param name="p_MaxCount">
        /// 0 이하면 무한 반복 모드, 양수면 해당 횟수만큼 반복 후 Success.
        /// </param>
        public RepeaterNode(TOwner p_Owner, Blackboard p_Blackboard, INode p_Child, int p_MaxCount)
            : base(p_Owner, p_Blackboard, p_Child)
        {
            m_MaxCount = p_MaxCount;
            m_CurrentCount = 0;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_Child == null)
            {
                return INode.NodeState.Failure;
            }

            INode.NodeState m_State = m_Child.Evaluate();

            if (m_State == INode.NodeState.Running)
            {
                return INode.NodeState.Running;
            }

            // Success 또는 Failure 한 번 끝난 것
            m_CurrentCount++;

            // 무한 반복 모드
            if (m_MaxCount <= 0)
            {
                return INode.NodeState.Running;
            }

            // 아직 횟수 덜 채움 → 계속 반복
            if (m_CurrentCount < m_MaxCount)
            {
                return INode.NodeState.Running;
            }

            // 횟수 채움 → Success로 종료 후 카운터 리셋
            m_CurrentCount = 0;
            return INode.NodeState.Success;
        }
    }
}