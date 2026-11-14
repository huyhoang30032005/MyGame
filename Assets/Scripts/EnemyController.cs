using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;

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

        nextFireTime = Time.time + fireCooldown;
    }

    void Update()
    {
        // Đến thời gian bắn
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }

        Patrol();
    }

    void Patrol()
    {
        float direction = movingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

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

    void Shoot()
    {
        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        float direction = movingRight ? 1f : -1f;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(direction);
    }
}