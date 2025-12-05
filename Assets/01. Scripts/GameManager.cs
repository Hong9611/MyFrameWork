using UnityEngine;

public interface IGameManager
{
    void TimeStop();
}

public class GameManager : IGameManager
{
    void Awake()
    {

    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }
}