using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float lifeTime = 10f;

    [Header("Combat")]
    [SerializeField] private int damage = 5;
    [SerializeField] private LayerMask targetLayer;

    [Header("Debug")]
    [SerializeField] private bool showDebugLog;

    private Rigidbody2D projectileRigidBody;
    private Vector2 moveDirection = Vector2.right;
    private GameObject owner;

    private void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody2D>();
        if (projectileRigidBody != null)
        {
            projectileRigidBody.gravityScale = 0f;
        }
    }
    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }
    public void Initialize(Vector2 direction, int projectileDamage, LayerMask projectileTargetLayer, GameObject projectileOwner)
    {
        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        moveDirection = direction.normalized;
        damage = projectileDamage;
        targetLayer = projectileTargetLayer;
        owner = projectileOwner;


        float zRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
    private void FixedUpdate()
    {
        if (projectileRigidBody != null)
        {
            projectileRigidBody.linearVelocity = moveDirection * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        if (((1 << collision.gameObject.layer) & targetLayer) == 0)
        {
            return;
        }

        PlayerHp playerHp = collision.GetComponent<PlayerHp>();
        if (playerHp == null)
        {
            playerHp = collision.GetComponentInParent<PlayerHp>();
        }

        if (playerHp != null && !playerHp.IsDead)
        {
            playerHp.TakePlayerDamage(damage);

            if (showDebugLog)
            {
                Debug.Log($"플레이어 타격! 데미지 {damage}");
            }

            Destroy(gameObject);
            return;
        }

        HPManager hp = collision.GetComponent<HPManager>();
        if (hp == null)
        {
            hp = collision.GetComponentInParent<HPManager>();
        }

        if (hp != null && !hp.IsDead)
        {
            hp.TakeDamage(damage);
        }
            Destroy(gameObject);
    }
}
