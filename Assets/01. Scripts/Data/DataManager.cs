using System.IO;
using UnityEngine;

public interface IDataManager
{
    GameData m_GameData { get; }

    void LoadLocalData();
    void SaveLocalData();
}

public class DataManager : IDataManager
{
    private string m_LocalDataSavePath;

    public GameData m_GameData { get; private set; }

    public DataManager()
    {
        m_LocalDataSavePath = Path.Combine(Application.persistentDataPath, "game_data.json");
        LoadLocalData();
    }

    public void LoadLocalData()
    {
        if (!File.Exists(m_LocalDataSavePath))
        {
            m_GameData = CreateDefaultData();
            SaveLocalData();
            Debug.Log("DataManager: 기본 게임 데이터 생성 및 저장 완료");
            return;
        }

        string json = File.ReadAllText(m_LocalDataSavePath);
        m_GameData = JsonUtility.FromJson<GameData>(json);

        Debug.Log("DataManager: 게임 데이터 로드 완료");
    }

    public void SaveLocalData()
    {
        if (m_GameData == null)
        {
            Debug.LogWarning("DataManager: 저장할 GameData 가 없습니다.");
            return;
        }

        string json = JsonUtility.ToJson(m_GameData, true);
        File.WriteAllText(m_LocalDataSavePath, json);

        Debug.Log("DataManager: 게임 데이터 저장 완료");
    }

    private GameData CreateDefaultData()
    {
        GameData data = new GameData();

        // 기본 사운드 볼륨 설정
        data.SoundVolumeData = new SoundVolumeData();

        int soundCount = System.Enum.GetValues(typeof(SoundType)).Length;

        for (int i = 1; i < soundCount; i++)
        {
            data.SoundVolumeData.m_Volumes.Add(1.0f);
            data.SoundVolumeData.m_Mutes.Add(false);
        }

        return data;
    }
}