using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 10;

    private float direction = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(float newDirection)
    {
        direction = newDirection;

        // Lật viên đạn khi bay sang trái
        if (direction < 0)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void Update()
    {
        transform.Translate(
            Vector2.right * direction * speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đạn có chạm Player không
        if (other.CompareTag("Player"))
        {
            ShinobiHealth shinobiHealth =
                other.GetComponentInParent<ShinobiHealth>();

            if (shinobiHealth != null)
            {
                shinobiHealth.TakeDamage(damage);
            }

            // Đạn biến mất sau khi chạm Player
            Destroy(gameObject);
        }
    }
}