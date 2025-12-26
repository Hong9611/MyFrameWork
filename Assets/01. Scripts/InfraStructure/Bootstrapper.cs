using UnityEngine;
using UnityEngine.Audio;

public class Bootstrapper : MonoBehaviour
{
    public static DiContainer Container { get; private set; }
    [SerializeField] private AudioMixer m_MainMixer;

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
            var dataManager = c.Resolve<IDataManager>();

            return new SoundManager(
                dataManager,   // DI로 받은 DataManager
                m_MainMixer    // Inspector에서 할당한 AudioMixer
            );
        });

        //ObjectPool 관련
        p_Container.RegisterSingleton<IObjectPoolManager, ObjectPoolManager>();

        // 필요하면 계속 추가
    }
}