using System.Collections.Generic;
using UnityEngine;

public class FlyMonsterSpawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform player;

    [Header("Pattern List")]
    [SerializeField] private List<GameObject> monsterType = new List<GameObject>();

    [Header("Spawn Time Settings")]
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float decreasePerStage = 0.2f;
    [SerializeField] private float minSpawnInterval = 1f;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRangeX = 5f;// 좌우 랜덤 범위
    [SerializeField] private float minSpawnDistanceY = 8f;// 플레이어 아래 최소 거리
    [SerializeField] private float maxSpawnDistanceY = 12f;// 플레이어 아래 최대 거리

    [Header("Spawn Limit")]
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private float monsterCheckRadius = 1.5f;
    [SerializeField] private int spawnTryCount = 8;

    private float currentSpawnInterval;
    private float timer;

    private void Awake()
    {
        if (player == null && DontDestroyPlayer.Instance != null)
            player = DontDestroyPlayer.Instance.transform;
    }

    private void Start()
    {
        ApplyStageDifficulty();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnMonster();
            timer = 0f;
        }
    }

    private void ApplyStageDifficulty()
    {
        int stageIndexInGroup = 0;

        if (StageProgressManager.Instance != null)
        {
            stageIndexInGroup = StageProgressManager.Instance.GetStageIndexInGroup();
        }

        currentSpawnInterval = baseSpawnInterval - (stageIndexInGroup * decreasePerStage);
        currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval);
    }

    private void SpawnMonster()
    {
        for (int i = 0; i < spawnTryCount; i++)
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomY = Random.Range(minSpawnDistanceY, maxSpawnDistanceY);

            Vector3 spawnPos = new Vector3( player.position.x + randomX, player.position.y - randomY, 0f );

            Collider2D nearbyMonster = Physics2D.OverlapCircle( spawnPos, monsterCheckRadius, monsterLayer );

            if (nearbyMonster != null)
                continue;

            int index = Random.Range(0, monsterType.Count);
            GameObject monsterPrefab = monsterType[index];

            GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

            MonsterHp monsterHp = monster.GetComponentInChildren<MonsterHp>();
            if (monsterHp != null)
            {
                monsterHp.ApplyStageHp();
            }

            return;
        }
    }
}