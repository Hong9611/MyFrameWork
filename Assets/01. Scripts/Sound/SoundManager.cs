using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public interface ISoundManager
{
    void SetAudioVolume(SoundType p_SoundType, float p_Volume, bool p_IsFromMute = false);
    void SetAudioMute(SoundType p_SoundType, bool p_IsMute);
    void PlaySound(SoundType p_SoundType, AudioClip p_Clip, bool p_Loop = false);
    void StopSound(SoundType p_SoundType);
}

public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private AudioMixer m_MainMixer;

    private List<AudioSource> m_AudioSources;
    private List<bool> m_IsMute;
    private List<float> m_AudioVolumes;

    // DI로 받아올 DataManager
    private IDataManager m_DataManager;

    public List<bool> IsMute
    {
        get { return m_IsMute; }
        set { m_IsMute = value; }
    }

    private void Start()
    {
        m_DataManager = Bootstrapper.Container.Resolve<IDataManager>();
        InitAudioSources();
    }

    private void InitAudioSources()
    {
        int soundCount = Enum.GetValues(typeof(SoundType)).Length;
        m_AudioSources = new List<AudioSource>(soundCount);
        m_IsMute = m_DataManager.m_GameData.SoundVolumeData.m_Mutes;
        m_AudioVolumes = m_DataManager.m_GameData.SoundVolumeData.m_Volumes;

        for (int i = 1; i < soundCount; i++)
        {
            SoundType type = (SoundType)i;

            GameObject obj = new GameObject(type.ToString());
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = m_MainMixer.FindMatchingGroups(type.ToString())[0];

            m_AudioSources.Add(source);
        }
    }

    public void PlaySound(SoundType p_SoundType, AudioClip p_Clip, bool p_Loop = false)
    {
        if (p_SoundType == SoundType.None) return;

        int type = (int)p_SoundType - 1;
        AudioSource source = m_AudioSources[type];
        if (source == null) return;

        source.clip = p_Clip;
        source.loop = p_Loop;
        source.Play();
    }

    public void StopSound(SoundType p_SoundType)
    {
        if (p_SoundType == SoundType.None) return;

        int type = (int)p_SoundType - 1;
        m_AudioSources[type]?.Stop();
    }

    public void SetAudioVolume(SoundType p_SoundType, float p_Volume, bool p_IsFromMute = false)
    {
        if (p_SoundType == SoundType.None) return;

        float db = Mathf.Log10(Mathf.Clamp(p_Volume, 0.0001f, 1f)) * 20;
        m_MainMixer.SetFloat(p_SoundType.ToString(), db);

        if (!p_IsFromMute)
            m_AudioVolumes[(int)p_SoundType - 1] = p_Volume;
    }

    public void SetAudioMute(SoundType p_SoundType, bool p_IsMute)
    {
        if (p_SoundType == SoundType.None) return;

        int type = (int)p_SoundType - 1;

        m_IsMute[type] = p_IsMute;

        if (p_IsMute)
        {
            m_MainMixer.GetFloat(p_SoundType.ToString(), out float curVolume);
            m_AudioVolumes[type] = Mathf.Pow(10, curVolume / 20);
            SetAudioVolume(p_SoundType, 0.001f, p_IsMute);
        }
        else
        {
            SetAudioVolume(p_SoundType, m_AudioVolumes[type]);
        }
    }
}