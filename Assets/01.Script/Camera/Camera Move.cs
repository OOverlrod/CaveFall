using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    [Header("target")]
    [SerializeField] private Transform target;
    [SerializeField] private PlayerMove2D playerMove2D;
    [SerializeField] private Rigidbody2D rb;// 플레이어 속도에 따른 카메라이동 추가

    [Header("Follow Control")]
    [SerializeField] private bool startMoveCamera = false;

    [Header("moveSpeed")]
    [SerializeField] private float maxFollowSpeed = 10f;// 플레이어 속토에 따른 카메라이동 추가
    [SerializeField] private float minFollowSpeed = 2f;// 플레이어 속토에 따른 카메라이동 추가
    [SerializeField] private float speedInfluence = 0.5f;// 플레이어 속토에 따른 카메라이동 추가
    //[SerializeField] private float smoothSpeed = 5f;

    [Header("Offset")]
    [SerializeField] private float groundedOffset = 0f; // 땅 위
    [SerializeField] private float fallingOffset = -3f; // 낙하 중

    [Header("Y Limit")]
    [SerializeField] private float maxY = 10f;
    [SerializeField] private float minY = -10f;


    private float fixedX;
    private float fixedZ;
    private bool moveCamera;

    private void Awake()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        moveCamera = startMoveCamera;
        AssignPlayerTarget();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerTarget();
    }

    private void AssignPlayerTarget()
    {
        moveCamera = startMoveCamera;

        if (DontDestroyPlayer.Instance != null)
        {
            target = DontDestroyPlayer.Instance.transform;
            playerMove2D = DontDestroyPlayer.Instance.Move;
            rb = DontDestroyPlayer.Instance.GetComponent<Rigidbody2D>();// 플레이어 속토에 따른 카메라이동 추가
        }
    }

    private void LateUpdate()
    {
        if (!moveCamera) return;
        if (target == null) return;

        float currentOffset = groundedOffset;

        if (playerMove2D != null && !playerMove2D.IsGrounded)
        {
            currentOffset = fallingOffset;
        }
        
        float targetY = target.position.y + currentOffset;// 플레이어 Y 가져오기
        float clampedY = Mathf.Clamp(targetY, minY, maxY);// Y제한 걸기

        Vector3 targetPosition = new Vector3(fixedX, clampedY, fixedZ);

        float currentFollowSpeed = GetDynamicFollowSpeed();// 플레이어 속토에 따른 카메라이동 추가

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            currentFollowSpeed * Time.deltaTime // smoothSpeed -> currentFollowSpeed 플레이어 속토에 따른 카메라이동 으로 수정
        );
    }
    private float GetDynamicFollowSpeed() // 플레이어 속토에 따른 카메라이동 추가
    {
        if (rb == null)
            return minFollowSpeed;

        float playerSpeed = Mathf.Abs(rb.linearVelocity.y);

        float dynamicSpeed = minFollowSpeed + (playerSpeed * speedInfluence);

        return Mathf.Clamp(dynamicSpeed, minFollowSpeed, maxFollowSpeed);
    }
    public void StartFollowY()
    {
        moveCamera = true;
    }
    public void StopFollowY()
    {
        moveCamera = false;
    }
}
