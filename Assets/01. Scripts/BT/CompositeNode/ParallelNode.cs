namespace Project.BehaviourTree
{
    public enum ParallelSuccessPolicy
    {
        RequireAllSuccess,  // 모든 자식이 Success여야 Parallel이 Success
        RequireOneSuccess   // 하나라도 Success면 Parallel이 Success
    }

    public enum ParallelFailurePolicy
    {
        RequireAllFailure,  // 모든 자식이 Failure여야 Parallel이 Failure
        RequireOneFailure   // 하나라도 Failure면 Parallel이 Failure
    }

    /// <summary>
    /// 여러 자식을 동시에 Evaluate하는 Parallel 노드입니다.
    /// </summary>
    public class ParallelNode<TOwner> : ICompositeNode<TOwner> where TOwner : class
    {
        private readonly ParallelSuccessPolicy m_SuccessPolicy;
        private readonly ParallelFailurePolicy m_FailurePolicy;

        public ParallelNode(
            TOwner p_Owner,
            Blackboard p_Blackboard,
            ParallelSuccessPolicy p_SuccessPolicy,
            ParallelFailurePolicy p_FailurePolicy)
            : base(p_Owner, p_Blackboard)
        {
            m_SuccessPolicy = p_SuccessPolicy;
            m_FailurePolicy = p_FailurePolicy;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_Children == null || m_Children.Count == 0)
            {
                return INode.NodeState.Success;
            }

            int m_SuccessCount = 0;
            int m_FailureCount = 0;
            bool m_HasRunning = false;

            foreach (INode m_Child in m_Children)
            {
                if (m_Child == null)
                {
                    continue;
                }

                INode.NodeState m_State = m_Child.Evaluate();

                switch (m_State)
                {
                    case INode.NodeState.Success:
                        m_SuccessCount++;

                        if (m_SuccessPolicy == ParallelSuccessPolicy.RequireOneSuccess)
                        {
                            return INode.NodeState.Success;
                        }

                        break;

                    case INode.NodeState.Failure:
                        m_FailureCount++;

                        if (m_FailurePolicy == ParallelFailurePolicy.RequireOneFailure)
                        {
                            return INode.NodeState.Failure;
                        }

                        break;

                    case INode.NodeState.Running:
                        m_HasRunning = true;
                        break;
                }
            }

            // RequireAllSuccess: 모두 Success일 때만 성공
            if (m_SuccessPolicy == ParallelSuccessPolicy.RequireAllSuccess &&
                m_SuccessCount == m_Children.Count)
            {
                return INode.NodeState.Success;
            }

            // RequireAllFailure: 모두 Failure일 때만 실패
            if (m_FailurePolicy == ParallelFailurePolicy.RequireAllFailure &&
                m_FailureCount == m_Children.Count)
            {
                return INode.NodeState.Failure;
            }

            // 여기까지 왔는데 아무도 Running이 아니라면
            // (성공/실패가 섞여 있는 경우 등), 기본적으로 Failure로 본다.
            if (!m_HasRunning)
            {
                return INode.NodeState.Failure;
            }

            // 아직 Running인 자식이 하나라도 있으면 전체 Running
            return INode.NodeState.Running;
        }
    }
}