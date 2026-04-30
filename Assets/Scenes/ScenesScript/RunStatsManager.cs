using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    public static RunStatsManager Instance { get; private set; }

    public int MonsterKillCount { get; private set; }
    public int ReachedStage { get; private set; } = 1;
    public int DestroyedFloorCount { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMonsterKill()
    {
        MonsterKillCount++;
    }

    public void SetReachedStage(int stage)
    {
        ReachedStage = stage;
    }

    public void AddDestroyedFloor()
    {
        DestroyedFloorCount++;
    }

    public void ResetStats()
    {
        MonsterKillCount = 0;
        ReachedStage = 1;
        DestroyedFloorCount = 0;
    }
}