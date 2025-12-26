namespace Project.BehaviourTree
{
    /// <summary>
    /// 모든 자식이 순서대로 성공해야 Success가 되는 Sequence 노드입니다.
    /// </summary>
    public class SequenceNode<TOwner> : ICompositeNode<TOwner> where TOwner : class
    {
        private int m_CurrentIndex;

        public SequenceNode(TOwner p_Owner, Blackboard p_Blackboard)
            : base(p_Owner, p_Blackboard)
        {
            m_CurrentIndex = 0;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_Children.Count == 0)
            {
                // 할 일이 없으면 성공으로 간주
                return INode.NodeState.Success;
            }

            while (m_CurrentIndex < m_Children.Count)
            {
                INode m_CurrentChild = m_Children[m_CurrentIndex];
                if (m_CurrentChild == null)
                {
                    m_CurrentIndex++;
                    continue;
                }

                INode.NodeState m_State = m_CurrentChild.Evaluate();

                switch (m_State)
                {
                    case INode.NodeState.Running:
                        return INode.NodeState.Running;

                    case INode.NodeState.Failure:
                        m_CurrentIndex = 0;
                        return INode.NodeState.Failure;

                    case INode.NodeState.Success:
                        m_CurrentIndex++;
                        break;
                }
            }

            m_CurrentIndex = 0;
            return INode.NodeState.Success;
        }
    }
}