using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float distance = 3f; // Khoảng cách đi lên/xuống
    public float speed = 2f;    // Tốc độ di chuyển

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.PingPong(Time.time * speed, distance);

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}