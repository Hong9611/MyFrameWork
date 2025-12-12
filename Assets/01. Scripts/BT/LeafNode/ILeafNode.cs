namespace Project.BehaviourTree
{
    /// <summary>
    /// 자식이 없는 Leaf 노드의 베이스 클래스입니다.
    /// 실제 행동(Action) / 조건(Condition) 노드들이 이 클래스를 상속합니다.
    /// </summary>
    /// <typeparam name="TOwner">이 트리를 사용하는 에이전트 타입.</typeparam>
    public abstract class ILeafNode<TOwner> : BaseNode<TOwner> where TOwner : class
    {
        protected ILeafNode(TOwner p_Owner, Blackboard p_Blackboard)
            : base(p_Owner, p_Blackboard)
        {
        }
    }
}
