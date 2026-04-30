using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private DontDestroyPlayer myPlayer;

    private void Start()
    {
        if (myPlayer != null)
            myPlayer = GetComponentInParent<DontDestroyPlayer>();
        
        if (DontDestroyPlayer.Instance == null)
        {
            Debug.LogWarning("DontDestroyPlayer.Instance ¾øÀ½");
            return;
        }

        Transform player = DontDestroyPlayer.Instance.transform;
        player.position = transform.position;

        Rigidbody2D rb = DontDestroyPlayer.Instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}