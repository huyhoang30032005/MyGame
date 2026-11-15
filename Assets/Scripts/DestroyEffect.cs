using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destroyTime = 1f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}