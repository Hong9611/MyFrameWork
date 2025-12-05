using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSlider : MonoBehaviour
{
    private SoundType m_SoundType;
    private Slider m_Slider;
    private Toggle m_MuteToggle;
    private TMP_Text m_Text;

    private ISoundManager m_SoundManager;

    public void Init()
    {
        m_Slider = GetComponent<Slider>();
        m_MuteToggle = GetComponentInChildren<Toggle>();
        m_Text = GetComponentInChildren<TMP_Text>();

        m_SoundManager = Bootstrapper.Container.Resolve<ISoundManager>();

        m_Slider.onValueChanged.AddListener(OnVolumeChanged);
        m_MuteToggle.onValueChanged.AddListener(OnMuteToggle);
        NameTextSetting();
    }

    public void TypeSet(SoundType p_SoundType)
    {
        m_SoundType = p_SoundType;
    }

    private void NameTextSetting()
    {
        //나중에 로컬라이징 달 수도 있을 듯
        m_Text.text = m_SoundType.ToString();
    }

    private void OnVolumeChanged(float p_Value)
    {
        m_SoundManager.SetAudioVolume(m_SoundType, p_Value);
    }

    public void OnMuteToggle(bool p_IsMute)
    {
        m_SoundManager.SetAudioMute(m_SoundType, p_IsMute);
    }
}