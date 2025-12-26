using System;

namespace Project.BehaviourTree
{
    /// <summary>
    /// 델리게이트를 통해 조건을 평가하는 Leaf 노드입니다.
    /// true → Success, false → Failure를 반환합니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 에이전트 타입.</typeparam>
    public class ConditionNode<TOwner> : ILeafNode<TOwner> where TOwner : class
    {
        private readonly Func<TOwner, Blackboard, bool> m_ConditionFunc;

        public ConditionNode(
            TOwner p_Owner,
            Blackboard p_Blackboard,
            Func<TOwner, Blackboard, bool> p_ConditionFunc)
            : base(p_Owner, p_Blackboard)
        {
            m_ConditionFunc = p_ConditionFunc;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_ConditionFunc == null)
            {
                return INode.NodeState.Failure;
            }

            bool m_Result = m_ConditionFunc(m_Owner, m_Blackboard);
            return m_Result ? INode.NodeState.Success : INode.NodeState.Failure;
        }
    }
}