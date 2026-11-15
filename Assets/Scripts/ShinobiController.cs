using UnityEngine;

public class ShinobiController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private float horizontalInput;
    private int jumpCount;
    private const int maxJumpCount = 2;

    private bool isGrounded;
    private bool facingRight = true;

    [Header("Attack Effect")]
    public Transform attackPoint;
    public GameObject fireSlashEffectPrefab;

    [Header("Attack 1")]
    public float attackRadius = 1f;
    public int attack1Damage = 20;
    public LayerMask enemyLayer;

    //Attack 2
    [Header("Attack 2")]
    public GameObject flyingSwordPrefab;

    //Attack 3
    [Header("Attack 3")]
    public GameObject fireArrowPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Nhận phím A/D
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Quay mặt theo hướng di chuyển
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        // Kiểm tra Shinobi có đang đứng trên mặt đất không
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Đứng trên mặt đất thì reset số lần nhảy
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Nhảy và Double Jump bằng W
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpCount++;
        }


        // Attack 1 - phím J
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack1");
        }

        // Attack 2 - phím K
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack2");
        }

        // Attack 3 - phím L
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("Attack3");
        }

        // Cập nhật Animator
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Di chuyển trái/phải
        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }

    // Hiển thị vùng GroundCheck trong Scene
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );

        // Hiển thị vùng Attack1
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(
                attackPoint.position,
                attackRadius
            );
        }

    }

    // Hàm này được gọi từ Animation Event trong Attack1 animation
    public void PlayAttack1Effect()
    {
        GameObject effect = Instantiate(
            fireSlashEffectPrefab,
            attackPoint.position,
            attackPoint.rotation
        );

        // Nếu Shinobi đang nhìn sang trái, lật hiệu ứng
        if (!facingRight)
        {
            Vector3 scale = effect.transform.localScale;
            scale.x *= -1;
            effect.transform.localScale = scale;
        }

        Attack1Hit();
    }

    //Attack 2
    public void PlayAttack2Effect()
    {
        GameObject sword = Instantiate(
            flyingSwordPrefab,
            attackPoint.position,
            Quaternion.identity
        );

        FlyingSword flyingSword = sword.GetComponent<FlyingSword>();

        if (flyingSword != null)
        {
            int direction = facingRight ? 1 : -1;
            flyingSword.SetDirection(direction);
        }
    }

    public void PlayAttack3Effect()
    {
        GameObject fireArrow = Instantiate(
            fireArrowPrefab,
            attackPoint.position,
            Quaternion.identity
        );

        FireArrow projectile =
            fireArrow.GetComponent<FireArrow>();

        if (projectile != null)
        {
            int direction = facingRight ? 1 : -1;
            projectile.SetDirection(direction);
        }
    }

    public void Attack1Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth =
                enemy.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attack1Damage);
            }
        }
    }
}