using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;

    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 8f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 2f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private float nextFireTime;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Tự động tìm Player bằng Tag
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        nextFireTime = Time.time + fireCooldown;
    }

    void Update()
    {
        bool playerDetected = false;

        // Kiểm tra Player có trong phạm vi không
        if (player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= detectionRange)
            {
                playerDetected = true;

                // Quay mặt về phía Player
                FacePlayer();

                // Bắn theo thời gian hồi chiêu
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireCooldown;
                }
            }
        }

        // Không phát hiện Player thì tuần tra
        if (!playerDetected)
        {
            Patrol();
        }
        else
        {
            // Khi đang phát hiện Player thì đứng yên
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            animator.SetFloat("Speed", 0);
        }
    }

    void Patrol()
    {
        float direction = movingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

        animator.SetFloat(
            "Speed",
            Mathf.Abs(rb.linearVelocity.x)
        );

        if (movingRight &&
            transform.position.x >= startPosition.x + patrolDistance)
        {
            movingRight = false;
            spriteRenderer.flipX = true;
        }
        else if (!movingRight &&
                 transform.position.x <= startPosition.x - patrolDistance)
        {
            movingRight = true;
            spriteRenderer.flipX = false;
        }
    }

    // Quay mặt về phía Player
    void FacePlayer()
    {
        if (player.position.x > transform.position.x)
        {
            // Player ở bên phải
            spriteRenderer.flipX = false;
        }
        else
        {
            // Player ở bên trái
            spriteRenderer.flipX = true;
        }
    }

    // Bắn đạn về phía Player
    void Shoot()
    {
        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Xác định hướng Player
        float direction =
            player.position.x > transform.position.x
            ? 1f
            : -1f;

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }

    // Hiển thị phạm vi phát hiện
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );
    }
}