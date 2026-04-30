using System;
using UnityEngine;

public class StageProgressManager : MonoBehaviour
{
    public static StageProgressManager Instance { get; private set; }

    [Header("Progress")]
    [SerializeField] private int currentLoopLevel = 0;

    public int CurrentLoopLevel => currentLoopLevel;

    public event Action<int> OnStageChanged;

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
    public void IncreaseLoopLevel()
    {
        currentLoopLevel++;
        OnStageChanged?.Invoke(currentLoopLevel);
    }

    public void ResetLoopLevel()
    {
        currentLoopLevel = 0;
        OnStageChanged?.Invoke(currentLoopLevel);
    }

    public int GetStageIndexInGroup()
    {
        return (currentLoopLevel - 1) % 5;
    }

    public int GetMonsterHpByStage()
    {
        return 1 + (currentLoopLevel - 1) / 5;
    }
}
