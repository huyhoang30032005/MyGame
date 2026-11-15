using UnityEngine;

public class ShinobiHealth : MonoBehaviour
{
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
        // Đã chết thì không nhận sát thương nữa
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // Còn sống → chạy animation Hurt
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Chạy animation chết
        animator.SetTrigger("Die");

        // Tắt script điều khiển nhân vật
        ShinobiController controller =
            GetComponent<ShinobiController>();

        if (controller != null)
        {
            controller.enabled = false;
        }
    }
}