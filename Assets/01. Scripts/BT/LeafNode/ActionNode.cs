using System;

namespace Project.BehaviourTree
{
    /// <summary>
    /// 델리게이트를 통해 실제 행동을 수행하는 Leaf 노드입니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 에이전트 타입.</typeparam>
    public class ActionNode<TOwner> : ILeafNode<TOwner> where TOwner : class
    {
        private readonly Func<TOwner, Blackboard, INode.NodeState> m_ActionFunc;

        public ActionNode(
            TOwner p_Owner,
            Blackboard p_Blackboard,
            Func<TOwner, Blackboard, INode.NodeState> p_ActionFunc)
            : base(p_Owner, p_Blackboard)
        {
            m_ActionFunc = p_ActionFunc;
        }

        public override INode.NodeState Evaluate()
        {
            if (m_ActionFunc == null)
            {
                return INode.NodeState.Failure;
            }

            return m_ActionFunc(m_Owner, m_Blackboard);
        }
    }
}