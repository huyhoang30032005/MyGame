using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private int direction = 1;

    public void SetDirection(int newDirection)
    {
        direction = newDirection;

        // Lật hình khi bay sang trái
        if (direction < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(
            Vector2.right * direction * speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth =
                other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20);
            }

            Destroy(gameObject);
        }
    }
}