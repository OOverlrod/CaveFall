using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    public static DontDestroyPlayer Instance { get; private set; }

    public PlayerMove2D Move { get; private set; }
    public PlayerDash Dash { get; private set; }
    public PlayerInvincibility Invincible { get; private set; }
    public HPManager Hp { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) // 중복 Object 제거
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; //참조 등록
        DontDestroyOnLoad(gameObject); // 씬변경해도 유지

        Move =GetComponent<PlayerMove2D>();
        Dash = GetComponent<PlayerDash>();
        Invincible = GetComponent<PlayerInvincibility>();
        Hp = GetComponent<HPManager>();
    }
}
