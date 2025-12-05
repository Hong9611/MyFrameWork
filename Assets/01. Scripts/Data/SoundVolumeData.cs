using System.Collections.Generic;

[System.Serializable]
public class SoundVolumeData
{
    public List<float> m_Volumes = new List<float>();
    public List<bool> m_Mutes = new List<bool>();
}