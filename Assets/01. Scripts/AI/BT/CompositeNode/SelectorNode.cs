namespace Project.BehaviourTree
{
    /// <summary>
    /// 여러 자식 중 하나라도 성공하면 Success가 되는 Selector 노드입니다.
    /// </summary>
    public class SelectorNode<TOwner> : ICompositeNode<TOwner> where TOwner : class
    {
        private int m_CurrentIndex;

        public SelectorNode(TOwner p_Owner, Blackboard p_Blackboard)
            : base(p_Owner, p_Blackboard)
        {
            m_CurrentIndex = 0;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_Children.Count == 0)
            {
                // 선택지 자체가 없으면 실패로 간주
                return INode.NodeState.Failure;
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

                    case INode.NodeState.Success:
                        m_CurrentIndex = 0;
                        return INode.NodeState.Success;

                    case INode.NodeState.Failure:
                        m_CurrentIndex++;
                        break;
                }
            }

            m_CurrentIndex = 0;
            return INode.NodeState.Failure;
        }
    }
}