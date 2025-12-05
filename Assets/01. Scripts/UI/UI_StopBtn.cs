using UnityEngine;
using UnityEngine.UI;

public class UI_StopBtn : MonoBehaviour
{
    private Button m_Btn;

    private IGameManager m_GameManager;

    private void Start()
    {
        m_Btn = GetComponent<Button>();
        m_Btn.onClick.AddListener(StopBtn);
        m_GameManager = Bootstrapper.Container.Resolve<IGameManager>();
    }

    public void StopBtn()
    {
        m_GameManager.TimeStop();
    }
}