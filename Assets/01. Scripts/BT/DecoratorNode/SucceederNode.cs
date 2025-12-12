namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식의 결과와 상관없이 상위에는 항상 Success를 반환하는 데코레이터입니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 에이전트 타입.</typeparam>
    public class SucceederNode<TOwner> : IDecoratorNode<TOwner> where TOwner : class
    {
        public SucceederNode(
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
                // 자식이 없으면 그냥 Success 처리
                return INode.NodeState.Success;
            }

            INode.NodeState m_State = m_Child.Evaluate();

            if (m_State == INode.NodeState.Running)
            {
                return INode.NodeState.Running;
            }

            // Success / Failure 모두 Success로 변환
            return INode.NodeState.Success;
        }
    }
}