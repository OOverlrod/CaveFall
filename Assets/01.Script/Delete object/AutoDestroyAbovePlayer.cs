using UnityEngine;

public class AutoDestroyAbovePlayer : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform player;

    [Header("Destroy Setting")]
    [SerializeField] private float destroyOffsetY = 12f;

    private void Awake()
    {
        if (player == null && DontDestroyPlayer.Instance != null)
            player = DontDestroyPlayer.Instance.transform;
    }

    private void Update()
    {
        if (player == null) return;

        if (transform.position.y > player.position.y + destroyOffsetY)
        {
            Destroy(gameObject);
        }
    }
}