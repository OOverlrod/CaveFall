using System.Collections.Generic;
using UnityEngine;

public class FloorPatternSpawner : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform player;

    [Header("Pattern List")]
    [SerializeField] private List<GameObject> floorpatterns = new List<GameObject>();

    [Header("Spawn Setting")]
    [SerializeField] private float startSpawnY = 0f;
    [SerializeField] private float spawnIntervalY = 4f;
    [SerializeField] private float spawnTriggerOffset = 10f;
    [SerializeField] private int initialSpawnCount = 5;

    private float nextSpawnY;
    private int lastIndex = -1;
    private bool hasInitialized = false;

    private void Awake()
    {
        if (player == null&& DontDestroyPlayer.Instance != null)
            player = DontDestroyPlayer.Instance.transform;
    }
    private void Start()
    {
        InitializeFloor();
    }
    public void InitializeFloor()
    {
        if (hasInitialized) return;
        hasInitialized = true;

        if (player == null)
        {
            Debug.LogWarning("player 없음");
            return;
        }

        if (floorpatterns == null || floorpatterns.Count == 0)
        {
            Debug.LogWarning("Floorprefab 비어 있음");
            return;
        }

        // 플레이어 현재 위치 기준으로 시작 위치 설정
        nextSpawnY = player.position.y + startSpawnY;

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnPattern();
        }
    }

    private void Update()
    {
        // 방어코드 
        if (player == null) return;
        if (floorpatterns == null || floorpatterns.Count == 0) return;
        //플레이어가 일정 이상 떨어질시 바닥 생성
        while (player.position.y < nextSpawnY + spawnTriggerOffset) {  SpawnPattern(); }
    }

    private void SpawnPattern()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, floorpatterns.Count);
        }
        // 이전과 동일한 프리펩일지 다시 뽑기
        while (randomIndex == lastIndex && floorpatterns.Count > 1);

        lastIndex = randomIndex;
        // List 에서 프리팹 선택
        GameObject selectedPattern = floorpatterns[randomIndex];
        // 프리팹 생성할 위치
        Vector3 spawnPosition = new Vector3(0, nextSpawnY, 0f);
        // 프리팹 생성
        Instantiate(selectedPattern, spawnPosition, Quaternion.identity);
        // 다음 생성할 위치로 이동
        nextSpawnY -= spawnIntervalY;
    }
}
