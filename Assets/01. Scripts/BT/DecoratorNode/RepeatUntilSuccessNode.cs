namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식 노드가 Success를 반환할 때까지 반복 실행하는 데코레이터입니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 에이전트 타입.</typeparam>
    public class RepeatUntilSuccessNode<TOwner> : IDecoratorNode<TOwner> where TOwner : class
    {
        public RepeatUntilSuccessNode(
            TOwner p_Owner,
            Blackboard p_Blackboard,
            INode p_Child)
            : base(p_Owner, p_Blackboard, p_Child)
        {
        }

        public override INode.NodeState Evaluate()
        {
            if (m_Child == null)
            {
                return INode.NodeState.Failure;
            }

            INode.NodeState m_State = m_Child.Evaluate();

            if (m_State == INode.NodeState.Success)
            {
                return INode.NodeState.Success;
            }

            // Failure / Running 모두 "아직 성공 안 함"으로 보고 계속 시도
            return INode.NodeState.Running;
        }
    }
}