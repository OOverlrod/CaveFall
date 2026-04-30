using System.Collections.Generic;
using UnityEngine;

public class GroundMonsterSpawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform player;

    [Header("Monster List")]
    [SerializeField] private List<GameObject> monsterTypes = new List<GameObject>();

    [Header("Spawn Time Settings")]
    [SerializeField] private float baseSpawnInterval = 3f;
    [SerializeField] private float decreasePerStage = 0.3f;
    [SerializeField] private float minSpawnInterval = 1f;

    [Header("Spawn Position Settings")]
    [SerializeField] private float minSpawnDistanceX = 2.5f;
    [SerializeField] private float maxSpawnDistanceX = 6f;
    [SerializeField] private float spawnOffsetY = 3f;
    [SerializeField] private float raycastDistance = 15f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Spawn Limit")]
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private float monsterCheckRadius = 1.2f;

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
        if (monsterTypes == null || monsterTypes.Count == 0 || player == null)
            return;

        for (int i = 0; i < 8; i++)
        {
            float randomX = Random.Range(minSpawnDistanceX, maxSpawnDistanceX);

            if (Random.value < 0.5f)
                randomX *= -1f;

            Vector2 rayStart = new Vector2( player.position.x + randomX, player.position.y - spawnOffsetY);

            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, raycastDistance, groundLayer);

            if (hit.collider == null)
                continue;

            if (hit.point.y >= player.position.y)
                continue;

            Vector3 spawnPos = hit.point;

            Collider2D nearbyMonster = Physics2D.OverlapCircle(spawnPos, monsterCheckRadius, monsterLayer);

            if (nearbyMonster != null)
                continue;

            int index = Random.Range(0, monsterTypes.Count);
            GameObject monsterPrefab = monsterTypes[index];

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