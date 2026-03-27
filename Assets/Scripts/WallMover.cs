using UnityEngine;

public class WallMover : MonoBehaviour
{
    [HideInInspector] public float speed = 3f;

    void Update()
    {
        // Always use latest speed from GameManager
        speed = GameManager.Instance.currentSpeed;

        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < -12f)
            Destroy(gameObject);
    }
}