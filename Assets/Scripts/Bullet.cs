using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

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
}