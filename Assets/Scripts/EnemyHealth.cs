using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Không nhận sát thương nếu đã chết
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // Vẫn còn sống → chạy animation Hurt
            animator.SetTrigger("Hurt");
        }
        else
        {
            // Hết máu → chết
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        animator.SetTrigger("Die");

        // Xóa Enemy sau khi animation chết chạy xong
        Destroy(gameObject, 1f);
    }
}