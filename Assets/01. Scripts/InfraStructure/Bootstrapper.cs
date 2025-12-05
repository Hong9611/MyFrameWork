using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    public static DiContainer Container { get; private set; }

    private void Awake()
    {
        // 이미 초기화된 Bootstrapper가 있다면 자신은 파괴
        if (Container != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Container = new DiContainer();
        RegisterServices(Container);
    }

    private void RegisterServices(DiContainer p_Container)
    {
        // 여기에서 프로젝트에 필요한 서비스들을 등록

        // Data 관련
        p_Container.RegisterSingleton<IDataManager, DataManager>();

        // Gameplay 관련
        p_Container.RegisterSingleton<IGameManager, GameManager>();

        //Sound 관련
        p_Container.RegisterSingleton<ISoundManager>(c =>
        {
            var soundManager = GameObject.FindObjectOfType<SoundManager>();
            if (soundManager == null)
            {
                Debug.LogError("SoundManager를 씬에서 찾을 수 없습니다. 씬에 SoundManager가 있어야 합니다.");
            }
            return soundManager;
        });

        //ObjectPool 관련
        p_Container.RegisterSingleton<IObjectPoolManager, ObjectPoolManager>();

        // 필요하면 계속 추가
    }
}