using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 인스턴스가 저장되는 정적 변수
    private static T m_Instance;

    // 스레드 안전성을 위한 lock 객체
    private static readonly object m_Lock = new object();

    // 애플리케이션 종료 시점 확인용 (종료 중에 새 인스턴스 생성 방지)
    private static bool m_IsQuitting = false;

    /// <summary>
    /// 싱글톤 인스턴스 접근자 (전역 접근 포인트)
    /// </summary>
    public static T Instance
    {
        get
        {
            // 종료 중일 때는 null 반환 (에디터 종료 시 에러 방지)
            if (m_IsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Returning null.");
                return null;
            }

            // 스레드 동기화 (멀티스레드 환경에서 안전)
            lock (m_Lock)
            {
                // 인스턴스가 아직 없다면 찾거나 생성
                if (m_Instance == null)
                {
                    // 현재 씬에서 해당 타입의 오브젝트 검색
                    m_Instance = FindObjectOfType<T>();

                    // 동일 타입의 객체가 2개 이상 존재할 경우 오류 경고
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError($"[Singleton] Multiple instances of singleton {typeof(T)} detected!");
                        return m_Instance;
                    }

                    // 아무것도 없을 경우 새 GameObject를 만들어 부착
                    if (m_Instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T)} (Singleton)";
                        DontDestroyOnLoad(singletonObject);

                        Debug.Log($"[Singleton] An instance of {typeof(T)} was created automatically.");
                    }
                }

                return m_Instance;
            }
        }
    }

    /// <summary>
    /// 유니티의 Awake() 호출 시 싱글톤 초기화 처리
    /// </summary>
    protected virtual void Awake()
    {
        // 아직 인스턴스가 없으면 자신을 등록
        if (m_Instance == null)
        {
            m_Instance = this as T;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않음
        }
        // 이미 존재하는 경우 중복 인스턴스 제거
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 애플리케이션 종료 시점 감지
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        m_IsQuitting = true;
    }

    /// <summary>
    /// 오브젝트 파괴 시점 처리 (메모리 정리)
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
        {
            m_IsQuitting = true;
            m_Instance = null;
        }
    }
}