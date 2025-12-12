namespace Project.BehaviourTree
{
    /// <summary>
    /// 모든 BT 노드가 구현해야 하는 기본 인터페이스입니다.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 노드의 실행 결과 상태입니다.
        /// </summary>
        public enum NodeState
        {
            /// <summary>조건 불충족, 수행 불가.</summary>
            Failure,

            /// <summary>할 일을 모두 성공적으로 끝냄.</summary>
            Success,

            /// <summary>여전히 진행 중. 다음 Tick에서 다시 Evaluate 해야 함.</summary>
            Running
        }

        /// <summary>
        /// 노드를 한 번 실행(Tick)합니다.
        /// </summary>
        /// <remarks>
        /// 매 프레임 혹은 일정 주기로 호출되며, <see cref="NodeState"/>를 반환합니다.
        /// </remarks>
        NodeState Evaluate();
    }
}