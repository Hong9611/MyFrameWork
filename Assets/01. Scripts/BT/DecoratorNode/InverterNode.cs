namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식의 Success/Failure를 뒤집는 Inverter 데코레이터입니다.
    /// </summary>
    public class InverterNode<TOwner> : IDecoratorNode<TOwner> where TOwner : class
    {
        public InverterNode(TOwner p_Owner, Blackboard p_Blackboard, INode p_Child)
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

            switch (m_State)
            {
                case INode.NodeState.Success:
                    return INode.NodeState.Failure;

                case INode.NodeState.Failure:
                    return INode.NodeState.Success;

                case INode.NodeState.Running:
                default:
                    return INode.NodeState.Running;
            }
        }
    }
}